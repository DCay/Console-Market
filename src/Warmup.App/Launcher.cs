using System.Reflection;
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
        // 1. Seed Roles into Database
        //    * Admin - Full permissions - manage products / receipts / cashDecks / ProductStorage / ProductDisplay
        //    * Senior Cashier - Manage Receipts (service clients) / CashDecks / Product Storage / Product Display
        //    * Junior Cashier - Manage Receipts (service clients)
        //    * Client - Manage his own cart
        // 2. Implement roles authorization
        // 3. Implement Product Creation
        // 4. Implement Product Storage Creation
        // 5. Implement Product Display Creation
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