using Warmup.App.Core.Base.Views;
using Warmup.App.Data;
using Warmup.App.Common.Attributes;
using Warmup.App.Core.Views.User;
using Warmup.App.Core.Views.Core;
using Warmup.App.Data.Entities;
using Warmup.App.Core.Models.Core;
using Warmup.App.Core.Models.User;

namespace Warmup.App.Core.Controllers
{
    public class UserController : Controller
    {
        private readonly WarmupDbContext warmupDbContext;

        private readonly Authentication authentication;

        public UserController(WarmupDbContext warmupDbContext, Authentication authentication)
        {
            this.warmupDbContext = warmupDbContext;
            this.authentication = authentication;
        }

        [CommandAlias("/register")]
        [CommandDescription("Registers a User into the system")]
        [CommandUsage("/register {username} {password} {confirmPassword}")]
        public IView Register(string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                Console.WriteLine("Passwords do not match...");
                return new BlankView();
            }

            this.warmupDbContext.Users.Add(new User
            {
                Username = username,
                Password = password,
                Role = null
            });

            return new RegisterView
            {
                ViewData = new Dictionary<string, object>
                {
                    ["username"] = username
                }
            };
        }

        [CommandAlias("/login")]
        [CommandDescription("Logs in a User into the system")]
        [CommandUsage("/login {username} {password}")]
        public IView Login(string username, string password)
        {
            if (!this.warmupDbContext.Users.Any(x => x.Username == username && x.Password == password))
            {
                Console.WriteLine("User does not exist...");
                return new BlankView();
            }

            this.authentication.User = username;
            this.authentication.IsAuthenticated = true;
            this.authentication.SessionData.Add(username + "-cart", new UserCart());

            return new LoginView
            {
                ViewData = new Dictionary<string, object>
                {
                    ["username"] = username
                }
            };
        }

        [CommandAlias("/logout")]
        [CommandDescription("Logs out the current User from the system")]
        [CommandUsage("Just type it")]
        public IView Logout()
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }
            
            this.authentication.User = null;
            this.authentication.IsAuthenticated = false;
            this.authentication.SessionData.Clear();

            return new LogoutView();
        }
    }
}
