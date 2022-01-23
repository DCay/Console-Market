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
            if (this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You cannot register a user when you are logged in...");
                return new BlankView();
            }

            if (password != confirmPassword)
            {
                Console.WriteLine("Passwords do not match...");
                return new BlankView();
            }

            if (this.warmupDbContext.Users.Any(x => x.Username == username))
            {
                Console.WriteLine("There is already a user with the given username...");
                return new BlankView();
            }

            UserRole userRole = null;

            if (this.warmupDbContext.Users.Count == 0)
            {
                userRole = this.warmupDbContext.Roles.FirstOrDefault(role => role.Name == "Admin");
            }
            else
            {
                userRole = this.warmupDbContext.Roles.FirstOrDefault(role => role.Name == "Client");
            }

            this.warmupDbContext.Users.Add(new User
            {
                Username = username,
                Password = password,
                Role = userRole
            });

            this.ViewData["username"] = username;

            return this.View();
        }

        [CommandAlias("/login")]
        [CommandDescription("Logs in a User into the system")]
        [CommandUsage("/login {username} {password}")]
        public IView Login(string username, string password)
        {
            if (this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are already logged in...");
                return new BlankView();
            }

            if (!this.warmupDbContext.Users.Any(x => x.Username == username && x.Password == password))
            {
                Console.WriteLine("User does not exist...");
                return new BlankView();
            }

            this.authentication.User = username;
            this.authentication.Role = this.warmupDbContext.Users.FirstOrDefault(x => x.Username == username).Role.Name;
            this.authentication.IsAuthenticated = true;
            this.authentication.SessionData.Add(username + "-cart", new UserCart());

            this.ViewData["username"] = username;

            return this.View();
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

            return this.View();
        }

        [CommandAlias("/authorize")]
        [CommandDescription("Assigns a given role to a user")]
        [CommandUsage("/authorize {username} {role}")]
        [CommandAuthority("Admin")]
        public IView Authorize(string username, string role)
        {
            if (!this.authentication.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in...");
                return new BlankView();
            }

            this.warmupDbContext.Users.FirstOrDefault(x => x.Username == username).Role 
                = this.warmupDbContext.Roles.FirstOrDefault(y => y.Name == role);

            this.ViewData["username"] = username;
            this.ViewData["role"] = role;

            return this.View();
        }
    }
}
