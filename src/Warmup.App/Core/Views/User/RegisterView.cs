using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.User
{
    public class RegisterView : BaseView
    {
        public RegisterView()
        {
        }
        
        public override string GetRepresentation()
        {
            return $"User - {this.ViewData["username"]} registered succesfully!";
        }
    }
}
