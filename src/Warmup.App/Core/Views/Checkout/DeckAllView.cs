using System.Globalization;
using System.Text;
using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class DeckAllView : BaseView
    {
        private readonly string LastLine = new string('#', 53);
        
        public override string GetRepresentation()
        {
            StringBuilder result = new StringBuilder(this.ViewData["view"].ToString());

            List<Data.Entities.CashDeck> decks = ((IEnumerable<Data.Entities.CashDeck>)this.ViewData["decks"]).ToList();

            if (decks.Count == 0)
            {
                result.AppendLine($"#{new string(' ', 23)}NONE{new string(' ', 24)}#");
            }
            else
            {
                foreach (var deck in decks)
                {
                    string deckCashierName = (deck.Cashier == null ? "CLOSED" : deck.Cashier.Username);
                    int deckQueue = ((Dictionary<int, int>) this.ViewData["deckQueues"])[deck.CashDeckIndex];

                    result.AppendLine($"# {deck.CashDeckIndex.ToString().PadRight(8)} # {deckCashierName.PadRight(30)} # {deckQueue.ToString().PadRight(5)} #");
                }
            }

            result.AppendLine(LastLine);

            return result.ToString();
        }
    }
}
