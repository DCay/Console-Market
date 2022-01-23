using System.Reflection;
using System.Text;
using Warmup.App.Common.Attributes;
using Warmup.App.Core.Base.Views;
using Warmup.App.Core.Models.Core;
using Warmup.App.Core.Views.Help;

namespace Warmup.App.Core.Controllers
{
    public class HelpController : Controller
    {
        private readonly Authentication authentication;

        public HelpController(Authentication authentication)
        {
            this.authentication = authentication;
        }

        [CommandAlias("/help")]
        [CommandDescription("Visualizes info about all commands")]
        [CommandUsage("Just type it")]
        public IView Help()
        {

            List<Type> controllerClasses = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type != typeof(Controller) && type.IsAssignableTo(typeof(Controller)))
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var controllerClass in controllerClasses)
            {
                List<MethodInfo> publicCommandMethods = controllerClass.GetMethods()
                   .Where(method => method.IsPublic && method
                       .GetCustomAttributes(typeof(CommandAliasAttribute))
                       .Any())
                   .ToList();

                foreach (var currentCommandMethod in publicCommandMethods)
                {
                    if (currentCommandMethod
                        .GetCustomAttributes()
                        .Any(attribute => attribute.GetType().FullName.Contains("CommandAlias")))
                    {
                        CommandAliasAttribute currentCommandAliasAttribute = (CommandAliasAttribute)
                            currentCommandMethod.GetCustomAttributes(typeof(CommandAliasAttribute)).FirstOrDefault();
                        CommandDescriptionAttribute currentCommandDescriptionAttribute = (CommandDescriptionAttribute)
                            currentCommandMethod.GetCustomAttributes(typeof(CommandDescriptionAttribute)).FirstOrDefault();
                        CommandUsageAttribute currentCommandUsageAttribute = (CommandUsageAttribute)
                            currentCommandMethod.GetCustomAttributes(typeof(CommandUsageAttribute)).FirstOrDefault();
                        CommandAuthorityAttribute currentCommandAuthorityAttribute = (CommandAuthorityAttribute)
                            currentCommandMethod.GetCustomAttributes(typeof(CommandAuthorityAttribute)).FirstOrDefault();

                        string formatPattern = "[{0}] -> {1}; Usage: {2}";

                        string commandAlias = currentCommandAliasAttribute.Alias;
                        string commandDescription = currentCommandDescriptionAttribute != null
                            ? currentCommandDescriptionAttribute.Description : "No info...";
                        string commandUsage = currentCommandUsageAttribute != null
                            ? currentCommandUsageAttribute.Usage : "No info...";

                        string commandRepresentation = string.Format(formatPattern, commandAlias, commandDescription, commandUsage);

                        if (currentCommandAuthorityAttribute == null ||
                            (currentCommandAuthorityAttribute != null && currentCommandAuthorityAttribute.Authorities.Contains(this.authentication.Role)))
                        {
                            result.AppendLine(commandRepresentation);
                        }
                    }
                }
            }

            this.ViewData["view"] = result.ToString();

            return this.View();
        }
    }
}
