using System.Reflection;

namespace Warmup.App.Core.Controllers
{
    public abstract class Controller
    {
        protected const string ResourcesBase = "../../../Resources";

        protected virtual string GetResource(string resource)
        {
            return File.ReadAllText(ResourcesBase + "/" + resource);
        }
    }
}
