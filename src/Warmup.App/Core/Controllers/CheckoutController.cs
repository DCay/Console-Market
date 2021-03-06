using Microsoft.EntityFrameworkCore;
using Warmup.App.Common.Attributes;
using Warmup.App.Core.Base.Views;
using Warmup.App.Core.Models.Checkout;
using Warmup.App.Core.Models.Core;
using Warmup.App.Core.Models.User;
using Warmup.App.Core.Views.Core;
using Warmup.App.Core.Views.Product;
using Warmup.App.Data;
using Warmup.App.Data.Entities;

namespace Warmup.App.Core.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly WarmupDbContext warmupDbContext;

        private readonly Authentication authentication;

        private readonly QueueManager queueManager;

        public CheckoutController(WarmupDbContext warmupDbContext, Authentication authentication, QueueManager queueManager)
        {
            this.warmupDbContext = warmupDbContext;
            this.authentication = authentication;
            this.queueManager = queueManager;
        }

        [CommandAlias("/deck-create")]
        [CommandDescription("Creates a cash deck")]
        [CommandUsage("/deck-create {index}")]
        [CommandAuthority("Admin")]
        public IView DeckCreate(string deckIndex)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            CashDeck deck = new CashDeck
            {
                CashDeckIndex = int.Parse(deckIndex),
                TotalMoney = 0M
            };

            this.warmupDbContext.CashDecks.Add(deck);
            this.warmupDbContext.SaveChanges();
            this.queueManager.DeckQueues.Add(deck.CashDeckIndex, new Queue<User>());

            this.ViewData["deckIndex"] = deckIndex;

            return this.View();
        }

        [CommandAlias("/deck-assign")]
        [CommandDescription("Assigns a cashier to a deck")]
        [CommandUsage("/deck-assign {index} {cashierName}")]
        [CommandAuthority("Admin", "SeniorCashier")]
        public IView DeckAssign(string deckIndex, string cashierName)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            CashDeck deck = this.warmupDbContext.CashDecks.FirstOrDefault(x => x.CashDeckIndex == int.Parse(deckIndex));
            User cashier = this.warmupDbContext.Users.Include(x => x.Role).FirstOrDefault(x => x.Username == cashierName);

            if (cashier.Role.Name != "Admin" && cashier.Role.Name != "SeniorCashier" && cashier.Role.Name != "JuniorCashier")
            {
                Console.WriteLine("The specified user cannot be assigned to a cash deck...");
                return new BlankView();
            }

            deck.Cashier = cashier;

            this.warmupDbContext.Update(deck);
            this.warmupDbContext.SaveChanges();

            this.ViewData["deckIndex"] = deckIndex;
            this.ViewData["cashierName"] = cashier.Username;
            this.ViewData["cashierRole"] = cashier.Role.Name;

            return this.View();
        }

        [CommandAlias("/deck-all")]
        [CommandDescription("Visualizes all decks")]
        [CommandUsage("Just type it")]
        public IView DeckAll()
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            this.ViewData["decks"] = this.warmupDbContext.CashDecks.ToList();
            this.ViewData["deckQueues"] = this.queueManager.DeckQueues.ToDictionary(x => x.Key, x => x.Value.Count);
            this.ViewData["view"] = this.GetResource("deck-all.txt");

            return this.View();
        }


        [CommandAlias("/deck-queue")]
        [CommandDescription("Enqueues the current user into the specified deck")]
        [CommandUsage("/deck-queue {index}")]
        public IView DeckQueue(string index)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            if (!this.queueManager.DeckQueues.ContainsKey(int.Parse(index)))
            {
                Console.WriteLine("Invalid queue index...");
                return new BlankView();
            }

            User user = this.warmupDbContext.Users.FirstOrDefault(x => x.Username == this.authentication.User);
            UserCart userCart = (UserCart)this.authentication.SessionData[this.authentication.User + "-cart"];

            if (userCart.productsAndQuantity.Count == 0)
            {
                Console.WriteLine("You have no products in your cart...");
                return new BlankView();
            }

            this.queueManager.DeckQueues[int.Parse(index)].Enqueue(user);

            this.ViewData["deckIndex"] = index;

            return this.View();
        }

        [CommandAlias("/deck-checkout")]
        [CommandDescription("Checks out the first user from the queue of the specified deck")]
        [CommandUsage("/deck-checkout {index}")]
        [CommandAuthority("Admin", "SeniorCashier", "JuniorCashier")]
        public IView DeckCheckout(string index)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            CashDeck deck = this.warmupDbContext.CashDecks.FirstOrDefault(cd => cd.CashDeckIndex == int.Parse(index));

            if (this.authentication.User != deck.Cashier.Username)
            {
                Console.WriteLine("You are not assigned to this deck...");
                return new BlankView();
            }

            if (this.queueManager.DeckQueues[deck.CashDeckIndex].Count == 0)
            {
                Console.WriteLine("There are no clients in the queue...");
                return new BlankView();
            }

            User client = this.queueManager.DeckQueues[deck.CashDeckIndex].Dequeue();
            UserCart clientCart = (UserCart)this.authentication.SessionData[client.Username + "-cart"];
            Dictionary<Product, int> productsAndQuantity = clientCart.productsAndQuantity
                .ToDictionary(x => this.warmupDbContext.Products.FirstOrDefault(p => p.Id == x.Key), x => x.Value);

            IView cartView = new CartView();

            cartView.ViewData["view"] = this.GetResource("cart.txt");
            cartView.ViewData["cart"] = productsAndQuantity;

            File.WriteAllText($"{this.authentication.User}-Receipt-" + DateTime.Now.ToLongTimeString, cartView.GetRepresentation());
            
            this.authentication.SessionData.Remove(client.Username + "-cart");
            
            return this.View();
        }
    }
}
