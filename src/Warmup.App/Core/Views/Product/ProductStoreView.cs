using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class ProductStoreView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully stored {this.ViewData["quantity"]} of {this.ViewData["productName"]}.";
        }
    }
}
