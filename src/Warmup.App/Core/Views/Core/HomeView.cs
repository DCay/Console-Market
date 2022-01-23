using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Core
{
    public class HomeView : BaseView
    {
        public override string GetRepresentation()
        {
            return this.GetFormattedView();
        }
    }
}
