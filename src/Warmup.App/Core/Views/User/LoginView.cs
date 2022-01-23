using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.User
{
    public class LoginView : BaseView
    {
        public LoginView()
        {
        }
        
        public override string GetRepresentation()
        {
            return $"Logged in successfully! Greetings, {this.ViewData["username"]}!";
        }
    }
}
