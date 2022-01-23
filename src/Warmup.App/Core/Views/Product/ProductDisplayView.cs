using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class ProductDisplayView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully displayed {this.ViewData["quantity"]} of {this.ViewData["productName"]}.";
        }
    }
}
