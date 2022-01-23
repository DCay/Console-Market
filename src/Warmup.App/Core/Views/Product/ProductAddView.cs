using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class ProductAddView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully added {this.ViewData["quantity"]} of {this.ViewData["product"]} to your cart.";
        }
    }
}
