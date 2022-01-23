using System.Globalization;
using System.Text;
using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class ProductAllView : BaseView
    {
        private readonly string LastLine = new string('#', 96);
        
        public override string GetRepresentation()
        {
            StringBuilder result = new StringBuilder(this.ViewData["view"].ToString());

            List<Data.Entities.Product> products = ((IEnumerable<Data.Entities.Product>)this.ViewData["products"]).ToList();

            if (products.Count == 0)
            {
                result.AppendLine($"#{new string(' ', 45)}NONE{new string(' ', 45)}#");
            }
            else
            {
                foreach (var product in products)
                {
                    result.AppendLine($"# {product.Id} # {product.Name.PadRight(30)} # ${product.Price.ToString("0.00").PadRight(6)} # {product.ExpirationDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)} #");
                }
            }

            result.AppendLine(LastLine);

            return result.ToString();
        }
    }
}
