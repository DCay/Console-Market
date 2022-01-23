using Warmup.App.Common.Attributes;
using Warmup.App.Core.Base.Views;
using Warmup.App.Core.Models.Core;
using Warmup.App.Core.Models.User;
using Warmup.App.Core.Views.Core;
using Warmup.App.Data;
using Warmup.App.Data.Entities;

namespace Warmup.App.Core.Controllers
{
    public class ProductController : Controller
    {
        private readonly WarmupDbContext warmupDbContext;

        private readonly Authentication authentication;

        public ProductController(WarmupDbContext warmupDbContext, Authentication authentication)
        {
            this.warmupDbContext = warmupDbContext;
            this.authentication = authentication;
        }

        [CommandAlias("/product-create")]
        [CommandDescription("Creates a product")]
        [CommandUsage("/product-create {productName} {productPrice} {expirationDate}")]
        [CommandAuthority("Admin")]
        public IView ProductCreate(string productName, string productPrice, string expirationDate)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            Product product = new Product
            {
                Name = productName,
                Price = decimal.Parse(productPrice),
                ExpirationDate = DateTime.Parse(expirationDate)
            };

            this.warmupDbContext.Products.Add(product);

            this.ViewData["productName"] = productName;

            return this.View();
        }

        [CommandAlias("/product-store")]
        [CommandDescription("Stores a specified quantity of units of a specified product into the product storage")]
        [CommandUsage("/product-store {productName} {productQuantity}")]
        [CommandAuthority("Admin", "SeniorCashier")]
        public IView ProductStore(string productName, string productQuantity)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            if (!this.warmupDbContext.Products.Any(p => p.Name == productName))
            {
                Console.WriteLine("There is no such product...");
                return new BlankView();
            }

            if (int.Parse(productQuantity) <= 0)
            {
                Console.WriteLine("Invalid quantity...");
                return new BlankView();
            }

            ProductStorage productStorage = new ProductStorage
            {
                Product = this.warmupDbContext.Products.FirstOrDefault(p => p.Name == productName),
                Quantity = int.Parse(productQuantity)
            };

            this.warmupDbContext.ProductStorages.Add(productStorage);

            this.ViewData["productName"] = productName;
            this.ViewData["quantity"] = productQuantity;

            return this.View();
        }

        [CommandAlias("/product-display")]
        [CommandDescription("Displays a specified quantity of units of a specified product, from the storage")]
        [CommandUsage("/product-display {productName} {productQuantity}")]
        [CommandAuthority("Admin", "SeniorCashier")]
        public IView ProductDisplay(string productName, string productQuantity)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            if (!this.warmupDbContext.ProductStorages.Any(p => p.Product.Name == productName))
            {
                Console.WriteLine("Product not present in the storage...");
                return new BlankView();
            }

            int parsedProductQuantity = int.Parse(productQuantity);

            if (parsedProductQuantity <= 0)
            {
                Console.WriteLine("Invalid quantity...");
                return new BlankView();
            }

            ProductStorage productStorage = this.warmupDbContext.ProductStorages.FirstOrDefault(ps => ps.Product.Name == productName);

            if(productStorage.Quantity < parsedProductQuantity)
            {
                Console.WriteLine("Insufficient quantity in storage...");
                return new BlankView();
            }

            productStorage.Quantity -= parsedProductQuantity;

            ProductDisplay productDisplay = new ProductDisplay
            {
                Product = productStorage.Product,
                Quantity = parsedProductQuantity
            };

            this.warmupDbContext.ProductDisplays.Add(productDisplay);

            this.ViewData["productName"] = productName;
            this.ViewData["quantity"] = productQuantity;

            return this.View();
        }

        [CommandAlias("/product-all")]
        [CommandDescription("Visualizes all products in the system")]
        [CommandUsage("Just type it")]
        public IView ProductAll()
        {
            this.ViewData["products"] = this.warmupDbContext.Products;
            this.ViewData["view"] = this.GetResource("product-all.txt");

            return this.View();
        }

        [CommandAlias("/product-storage")]
        [CommandDescription("Visualizes all products, currently stored in the storage")]
        [CommandUsage("Just type it")]
        [CommandAuthority("Admin", "SeniorCashier")]
        public IView ProductStorage()
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            this.ViewData["productStorages"] = this.warmupDbContext.ProductStorages;
            this.ViewData["view"] = this.GetResource("product-storage.txt");

            return this.View();
        }

        [CommandAlias("/product-displayed")]
        [CommandDescription("Visualizes all products, currently displayed")]
        [CommandUsage("Just type it")]
        public IView ProductDisplayed()
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            this.ViewData["productDisplays"] = this.warmupDbContext.ProductDisplays;
            this.ViewData["view"] = this.GetResource("product-displayed.txt");

            return this.View();
        }

        [CommandAlias("/product-add")]
        [CommandDescription("Adds a a specified quantity of units of a specified product to the user's Cart")]
        [CommandUsage("/product-add {productName} {productQuantity}")]
        public IView ProductAdd(string productName, string productQuantity)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            int requestedQuantity = int.Parse(productQuantity);
            ProductDisplay productDisplay = this.warmupDbContext.ProductDisplays.FirstOrDefault(productDisplay => productDisplay.Product.Name == productName);

            if (productDisplay == null || productDisplay.Quantity < requestedQuantity)
            {
                Console.WriteLine("Product is not displayed or there is not enough units to satisfy the requested quantity...");
                return new BlankView();
            }

            UserCart userCart = (UserCart)this.authentication.SessionData[this.authentication.User + "-cart"];

            if (!userCart.productsAndQuantity.ContainsKey(productDisplay.Product.Id))
            {
                userCart.productsAndQuantity[productDisplay.Product.Id] = 0;
            }
            
            userCart.productsAndQuantity[productDisplay.Product.Id] += requestedQuantity;

            productDisplay.Quantity -= requestedQuantity;

            if (productDisplay.Quantity == 0)
            {
                this.warmupDbContext.ProductDisplays = this.warmupDbContext.ProductDisplays.Where(x => x.Product.Name != productName).ToList();
            }

            this.ViewData["product"] = productDisplay.Product.Name;
            this.ViewData["quantity"] = requestedQuantity;

            return this.View();
        }

        [CommandAlias("/cart")]
        [CommandDescription("Visualizes all products in the current user's cart")]
        [CommandUsage("Just type it")]
        public IView Cart()
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            UserCart userCart = (UserCart)this.authentication.SessionData[this.authentication.User + "-cart"];
            Dictionary<Product, int> productsAndQuantity = userCart.productsAndQuantity
                .ToDictionary(x => this.warmupDbContext.Products.FirstOrDefault(p => p.Id == x.Key), x => x.Value);

            this.ViewData["cart"] = productsAndQuantity;
            this.ViewData["view"] = this.GetResource("cart.txt");

            return this.View();
        }
    }
}
