using Warmup.App.Common.Attributes;
using Warmup.App.Core.Base.Views;
using Warmup.App.Core.Models.Core;
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

            if (this.authentication.Role != "Admin")
            {
                Console.WriteLine("You are not authorized to do this...");
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

            if (this.authentication.Role != "Admin" && this.authentication.Role != "SeniorCashier")
            {
                Console.WriteLine("You are not authorized to do this...");
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

            if (this.authentication.Role != "Admin" && this.authentication.Role != "SeniorCashier")
            {
                Console.WriteLine("You are not authorized to do this...");
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
    }
}
