using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Checkout
{
    public class DeckCheckoutView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully checked out and generated receipt.";
        }
    }
}
