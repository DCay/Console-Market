using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Help
{
    public class HelpView : BaseView
    {
        public override string GetRepresentation()
        {
            return this.GetFormattedView();
        }
    }
}
