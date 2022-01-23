using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warmup.App.Core.Views.Base;

namespace Warmup.App.Core.Views.Product
{
    public class ProductCreateView : BaseView
    {
        public override string GetRepresentation()
        {
            return $"Successfully created product - {this.ViewData["productName"]}.";
        }
    }
}
