using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving;
using Autofac.Integration.WebApi;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Controllers;
using prepo.Api.Repo.Memory;
using prepo.Api.Services;

namespace prepo.Api
{
    public class IocConfig
    {
        public static IContainer ConfigureIoc(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            RegisterControllers(builder);
            RegisterServices(builder);
            MemoryRepoRegisterIoc.Register(builder);

            var container = builder.Build();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
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

            var models = typeof(PrepoRoot).Assembly.GetTypes()
                                      .Where(t => t.IsAssignableTo<DbObject>());

            foreach (var model in models)
            {
                builder.RegisterType(typeof(ResourceController<>).MakeGenericType(model));
            }
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            //builder.RegisterType<RootResourceRepository>();
            //builder.RegisterType<UserResourceRepository>();
            //builder.RegisterType<PersonaResourceRepository>();
            builder.RegisterType<ResourceChainBuilder>();
            builder.RegisterType<ResourceRepositoryFactory>();
            builder.RegisterGeneric(typeof(ResourceResolutionService<>)).As(typeof(IResourceResolutionService<>));
        }
    }
}