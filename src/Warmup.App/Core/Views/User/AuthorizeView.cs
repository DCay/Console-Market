using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.User
{
    public class AuthorizeView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Succesfully assigned Role - {this.ViewData["role"]} to {this.ViewData["username"]}!";
        }
    }
}
