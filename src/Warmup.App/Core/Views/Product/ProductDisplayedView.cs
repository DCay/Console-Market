using System.Globalization;
using System.Text;
using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class ProductDisplayedView : BaseView
    {
        private readonly string LastLine = new string('#', 68);
        
        public override string GetRepresentation()
        {
            StringBuilder result = new StringBuilder(this.ViewData["view"].ToString());

            List<Data.Entities.ProductDisplay> productDisplays = ((IEnumerable<Data.Entities.ProductDisplay>)this.ViewData["productDisplays"]).ToList();

            if (productDisplays.Count == 0)
            {
                result.AppendLine($"#{new string(' ', 31)}NONE{new string(' ', 31)}#");
            }
            else
            {
                foreach (var productStorage in productDisplays)
                {
                    result.AppendLine($"# {productStorage.Product.Name.PadRight(30)} # {productStorage.Quantity.ToString().PadRight(8)} # ${productStorage.Product.Price.ToString("0.00").PadRight(6)} # {productStorage.Product.ExpirationDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)} #");
                }
            }

            result.AppendLine(LastLine);

            return result.ToString();
        }
    }
}
