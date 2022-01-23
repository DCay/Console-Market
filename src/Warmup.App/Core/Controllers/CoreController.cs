using Warmup.App.Common.Attributes;
using Warmup.App.Core.Base.Views;
using Warmup.App.Core.Models.Core;
using Warmup.App.Core.Views.Core;

namespace Warmup.App.Core.Controllers
{
    public class CoreController : Controller
    {
        private readonly Authentication authentication;

        public CoreController(Authentication authentication)
        {
            this.authentication = authentication;
        }

        [CommandAlias("/home")]
        [CommandDescription("Goes to home page")]
        [CommandUsage("Just type it")]
        public IView Home()
        {
            Console.Clear();

            if(this.authentication.IsAuthenticated)
            {
                this.ViewData["username"] = this.authentication.User;
                this.ViewData["view"] = this.GetResource("headline-user.txt");

                return this.View();
            } 
            else
            {
                this.ViewData["view"] = this.GetResource("headline-guest.txt");

                return this.View();
            }
        }

        [CommandAlias("/clear")]
        [CommandDescription("Clears the current terminal")]
        [CommandUsage("Just type it")]
        public IView Clear()
        {
            return this.Home();
        }

        [CommandAlias("/exit")]
        [CommandDescription("Exits the application")]
        [CommandUsage("Just type it")]
        public IView Exit()
        {
            Environment.Exit(0);
            return null;
        }
    }
}
