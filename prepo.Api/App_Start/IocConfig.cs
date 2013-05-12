using System.Web.Http;
using Codeite.SamIoc;
using Codeite.SamIoc.WebApi;
using prepo.Api.Services;

namespace prepo.Api
{
    public class IocConfig
    {
        public static void ConfigureIoc(HttpConfiguration configuration)
        {
            ISamIoc container = SamIoc.CreateInstance();
            container.AutoRegisterConcreteTypes = true;

            RegisterServices(container);

            configuration.DependencyResolver = new SamIocWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ISamIoc container)
        {
            container.Register<ResourceRepository>();
        }
    }
}