using System.Reflection;
using Warmup.App.Common.Attributes;
using Warmup.App.Core.Base.Views;
using Warmup.App.Core.Controllers;
using Warmup.App.Core.Models.Core;
using Warmup.App.Data;

namespace Warmup.App
{
    // Engine
    public class StartUp
    {
        private Dictionary<Type, Object> singletonDependencyContainer = new Dictionary<Type, Object>();

        private void Configure()
        {
            this.singletonDependencyContainer.Add(typeof(WarmupDbContext), new WarmupDbContext());
            this.singletonDependencyContainer.Add(typeof(Authentication), new Authentication());
        }

        private string GetInput() => Console.ReadLine();

        private KeyValuePair<string, List<string>> ParseInput(string input)
        {
            string[] inputArgs = input.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            return new KeyValuePair<string, List<string>>(inputArgs[0], inputArgs.Skip(1).ToList());
        }

        private string InvokeAction(string commandName, List<string> commandArgs)
        {
            Type controllerHolder = Assembly.GetExecutingAssembly()
                   .GetTypes()
                   .Where(type => type != typeof(Controller) && type.IsAssignableTo(typeof(Controller)) && type.GetMethods()
                        .Any(method => method.GetCustomAttributes(typeof(CommandAliasAttribute))
                            .Any() && ((CommandAliasAttribute)method.GetCustomAttributes(typeof(CommandAliasAttribute))
                                .FirstOrDefault()).Alias == commandName
                            )
                        ).FirstOrDefault();

            if(controllerHolder == null)
            {
                return "Invalid or unsupported command...";
            }

            ConstructorInfo controllerConstructorInfo = controllerHolder.GetConstructors().ToList().First();

            List<ParameterInfo> controllerRequiredParameters = controllerConstructorInfo.GetParameters().ToList();
            List<Object> controllerGivenParameters = new List<object>();

            foreach (var parameter in controllerRequiredParameters)
            {
                if(this.singletonDependencyContainer.ContainsKey(parameter.ParameterType))
                {
                    controllerGivenParameters.Add(this.singletonDependencyContainer[parameter.ParameterType]);
                }
            }

            object controllerObject = Activator.CreateInstance(controllerHolder, controllerGivenParameters.ToArray());
            MethodInfo controllerMethod = controllerObject.GetType().GetMethods().Where(method => method.GetCustomAttributes(typeof(CommandAliasAttribute))
                            .Any() && ((CommandAliasAttribute)method.GetCustomAttributes(typeof(CommandAliasAttribute)).FirstOrDefault()).Alias == commandName)
                .FirstOrDefault();

            IView result = (IView) controllerMethod.Invoke(controllerObject, commandArgs == null ? null : commandArgs.ToArray());

            return result.GetRepresentation();
        }

        private void Run()
        {
            Console.WriteLine(this.InvokeAction("/home", null));
            
            while(true)
            {
                KeyValuePair<string, List<string>> command = this.ParseInput(this.GetInput());

                string result = this.InvokeAction(command.Key, command.Value).Trim();

                if (!string.IsNullOrEmpty(result))
                {
                    Console.WriteLine(result);
                }
            }
        }

        public void Start()
        {
            this.Configure();
            this.Run();
        }
    }
}
