using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Core
{
    public class UnauthorizedView : BaseView
    {
        public override string GetRepresentation()
        {
            return "You are not authorized to perform this operation!";
        }
    }
}
