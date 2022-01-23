using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.User
{
    public class LogoutView : BaseView
    {
        public override string GetRepresentation()
        {
            return "Successfully logged out. Have a nice day!";
        }
    }
}
