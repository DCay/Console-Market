using System.Reflection;
using System.Runtime.CompilerServices;
using Warmup.App.Common.Attributes;
using Warmup.App.Core.Controllers;

namespace Warmup.App
{
    public class Launcher
    {
        // Shopping System
        // Core Functionalities
        // User (employee)
        // Role (employee role) - Client, Junior Cashier, Senior Cashier, Admin
        // Products (CRUD)
        // Receipts (CRD)
        // CashDesks (CRUD)
        // ProductStorage (CRUD)
        // ProductDisplay (CRUD)
        // Overview:
        // Clients have a current cart. When the cart is checked out, a receipt is generated.





        // GOALS FOR NOW:
        // 5.5. Implement Product / Product Storage / Product Display visualization
        // 6. Implement Client add to Cart
        // 7. Implement CashDecks creation and assignment of cashier
        // 8. Cashout
        // 9. EF Core
        // 10. ASP.NET Core


        public static void Main(string[] args)
        {
            new StartUp().Start();
        }
    }
}