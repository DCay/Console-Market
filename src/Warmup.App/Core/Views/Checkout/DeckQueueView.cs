using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Checkout
{
    public class DeckQueueView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully queued on Deck - {this.ViewData["deckIndex"]}";
        }
    }
}
