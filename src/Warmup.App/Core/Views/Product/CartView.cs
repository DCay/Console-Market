using System.Globalization;
using System.Text;
using Warmup.App.Core.Models.User;
using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class CartView : BaseView
    {
        private readonly string LastLine = new string('#', 68);
        
        public override string GetRepresentation()
        {
            StringBuilder result = new StringBuilder(this.ViewData["view"].ToString());

            Dictionary<Data.Entities.Product, int> productsAndQuantity = (Dictionary<Data.Entities.Product, int>) this.ViewData["cart"];

            if (productsAndQuantity.Count == 0)
            {
                result.AppendLine($"#{new string(' ', 31)}NONE{new string(' ', 31)}#");
            }
            else
            {
                foreach (var productAndQuantity in productsAndQuantity)
                {
                    result.AppendLine($"# {productAndQuantity.Key.Name.PadRight(30)} # {productAndQuantity.Value.ToString().PadRight(8)} # ${productAndQuantity.Key.Price.ToString("0.00").PadRight(6)} # {productAndQuantity.Key.ExpirationDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)} #");
                }
            }

            result.AppendLine(LastLine);

            string totalLine = "Total Price: $" + productsAndQuantity.Sum(p => p.Key.Price * p.Value).ToString("0.00");

            result.AppendLine($"# {totalLine.PadLeft(64)} #");

            result.AppendLine(LastLine);

            return result.ToString();
        }
    }
}
