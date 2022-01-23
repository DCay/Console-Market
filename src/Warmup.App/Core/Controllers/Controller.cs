using System.Reflection;
using System.Runtime.CompilerServices;
using Warmup.App.Core.Base.Views;

namespace Warmup.App.Core.Controllers
{
    public abstract class Controller
    {
        protected const string ResourcesBase = "../../../Resources";

        protected Dictionary<string, object> ViewData = new Dictionary<string, object>();

        protected virtual string GetResource(string resource)
        {
            return File.ReadAllText(ResourcesBase + "/" + resource);
        }

        protected virtual IView View([CallerMemberName] string callerName = "")
        {
            IView view = (IView)Activator.CreateInstance(Assembly.GetExecutingAssembly()
                   .GetTypes()
                   .FirstOrDefault(type => type.Name == (callerName + "View")));

            view.ViewData = this.ViewData;

            return view;
        }
    }
}
