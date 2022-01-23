using Warmup.App.Core.Base.Views;

namespace Warmup.App.Core.Views.Base
{
    public abstract class BaseView : IView
    {
        public Dictionary<string, object> ViewData;

        protected virtual string GetFormattedView()
        {
            string view = (string) this.ViewData["view"];

            string formattedView = view + "";

            foreach (var dataElement in this.ViewData)
            {
                formattedView = formattedView.Replace("#{" + dataElement.Key + "}#", (string) dataElement.Value);
            }

            return formattedView;
        }

        public abstract string GetRepresentation();
    }
}
