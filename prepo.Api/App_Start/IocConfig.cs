using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using prepo.Api.Contracts.Services;
using prepo.Api.Repo.Memory;
using prepo.Api.Services;

namespace prepo.Api
{
    public class IocConfig
    {
        public static void ConfigureIoc(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            RegisterControllers(builder);
            RegisterServices(builder);
            MemoryRepoRegisterIoc.Register(builder);

            var container = builder.Build();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            var controllers = Assembly.GetExecutingAssembly()
                                      .GetTypes()
                                      .Where(t => t.IsAssignableTo<ApiController>());

            foreach (var controller in controllers)
            {
                builder.RegisterType(controller);
            }
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<RootResourceRepository>();
            builder.RegisterType<UserResourceRepository>();
            builder.RegisterType<PersonaResourceRepository>();
            builder.RegisterType<ResourceChainBuilder>();
        }
    }
}