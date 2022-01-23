using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Checkout
{
    public class DeckCreateView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully created Deck - {this.ViewData["deckIndex"]}";
        }
    }
}
