using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Checkout
{
    public class DeckAssignView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully assigned {this.ViewData["cashierRole"]} {this.ViewData["cashierName"]} to Deck - {this.ViewData["deckIndex"]}";
        }
    }
}
