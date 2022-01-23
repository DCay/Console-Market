using System.Reflection;
using Warmup.App.Common.Attributes;
using Warmup.App.Core.Base.Views;
using Warmup.App.Core.Controllers;
using Warmup.App.Core.Models.Checkout;
using Warmup.App.Core.Models.Core;
using Warmup.App.Core.Views.Core;
using Warmup.App.Data;
using Warmup.App.Data.Entities;

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
            this.singletonDependencyContainer.Add(typeof(QueueManager), new QueueManager());
        }

        private void SeedDatabase()
        {
            UserRole adminRole = new UserRole { Name = "Admin" };
            UserRole seniorCashierRole = new UserRole { Name = "SeniorCashier" };
            UserRole juniorCashierRole = new UserRole { Name = "JuniorCashier" };
            UserRole clientRole = new UserRole { Name = "Client" };

            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Roles.Add(adminRole);
            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Roles.Add(seniorCashierRole);
            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Roles.Add(juniorCashierRole);
            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Roles.Add(clientRole);

            User admin = new User
            {
                Username = "pesho",
                Password = "123",
                Role = adminRole
            };

            User seniorCashier = new User
            {
                Username = "gosho",
                Password = "123",
                Role = seniorCashierRole
            };

            User juniorCashier = new User
            {
                Username = "toshka",
                Password = "123",
                Role = juniorCashierRole
            };

            User client = new User
            {
                Username = "prakash",
                Password = "123",
                Role = clientRole
            };

            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Users.Add(admin);
            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Users.Add(seniorCashier);
            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Users.Add(juniorCashier);
            ((WarmupDbContext)this.singletonDependencyContainer[typeof(WarmupDbContext)]).Users.Add(client);
        }

        private string GetInput() => Console.ReadLine();

        private KeyValuePair<string, List<string>> ParseInput(string input)
        {
            string[] inputArgs = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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

            if (controllerHolder == null)
            {
                return "Invalid or unsupported command...";
            }

            ConstructorInfo controllerConstructorInfo = controllerHolder.GetConstructors().ToList().First();

            List<ParameterInfo> controllerRequiredParameters = controllerConstructorInfo.GetParameters().ToList();
            List<Object> controllerGivenParameters = new List<object>();

            foreach (var parameter in controllerRequiredParameters)
            {
                if (this.singletonDependencyContainer.ContainsKey(parameter.ParameterType))
                {
                    controllerGivenParameters.Add(this.singletonDependencyContainer[parameter.ParameterType]);
                }
            }

            object controllerObject = Activator.CreateInstance(controllerHolder, controllerGivenParameters.ToArray());
            MethodInfo controllerMethod = controllerObject.GetType().GetMethods().Where(method => method.GetCustomAttributes(typeof(CommandAliasAttribute))
                            .Any() && ((CommandAliasAttribute)method.GetCustomAttributes(typeof(CommandAliasAttribute)).FirstOrDefault()).Alias == commandName)
                .FirstOrDefault();

            string authenticationRole = ((Authentication)this.singletonDependencyContainer[typeof(Authentication)]).Role;
            CommandAuthorityAttribute commandAuthorityAttribute = ((CommandAuthorityAttribute)controllerMethod.GetCustomAttributes(typeof(CommandAuthorityAttribute)).FirstOrDefault());

            IView result = null;

            if (commandAuthorityAttribute == null || commandAuthorityAttribute.Authorities.Contains(authenticationRole))
            {
                result = (IView)controllerMethod.Invoke(controllerObject, commandArgs == null ? null : commandArgs.ToArray());
            }
            else
            {
                result = new UnauthorizedView();
            }

            return result.GetRepresentation();
        }

        private void Run()
        {
            Console.WriteLine(this.InvokeAction("/home", null));

            while (true)
            {
                try
                {
                    KeyValuePair<string, List<string>> command = this.ParseInput(this.GetInput());

                    string result = this.InvokeAction(command.Key, command.Value).Trim();

                    if (!string.IsNullOrEmpty(result))
                    {
                        Console.WriteLine(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Start()
        {
            this.Configure();
            this.SeedDatabase();
            this.Run();
        }
    }
}
