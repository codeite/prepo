using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Autofac;
using Autofac.Core.Lifetime;
using prepo.Api.Infrastructure;
using prepo.Api.Infrastructure.Reflecting;
using prepo.Api.Services;

namespace prepo.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IComponentContext container)
        {
            //var resolver = config.Services.GetHttpControllerTypeResolver();
            //config.Services.Replace(typeof(IHttpControllerTypeResolver), new MyHttpControllerTypeResolver(resolver));

            var defaultSelector = config.Services.GetHttpControllerSelector();
            config.Services.Replace(
                typeof(IHttpControllerSelector), new ResourceHttpControllerSelector(config, defaultSelector, new ResourceRepositoryFactory(container)));

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { controller = "Root", id = RouteParameter.Optional }
            //);
            
            //config.Routes.MapHttpRoute(
            //    name: "ParentDefaultApi",
            //    routeTemplate: "{parent}/{parentId}/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //   name: "root",
            //   routeTemplate: "{controller}",
            //   defaults: new { controller = "Resource", path = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{*path}",
                defaults: new { controller = "Resource", path = "" }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();


            var binderCache = new JsonModelBinderCache();
            RegisterJsonMediaFormatter(config, new HalJsonMediaTypeFormatter(binderCache));
            RegisterXmlMediaFormatter(config, new HalXmlMediaTypeFormatter(binderCache));
        }

        private static void RegisterXmlMediaFormatter(HttpConfiguration config, MediaTypeFormatter mediaTypeFormatter)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(mediaTypeFormatter);
        }

        public static void RegisterJsonMediaFormatter(HttpConfiguration config, MediaTypeFormatter mediaTypeFormatter)
        {
            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Add(mediaTypeFormatter);
        }
    }
}
