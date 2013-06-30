using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace prepo.Api.Infrastructure
{
    public class MyHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        private readonly IHttpControllerTypeResolver _resolver;

        public MyHttpControllerTypeResolver(IHttpControllerTypeResolver resolver)
        {
            _resolver = resolver;
        }


        public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            var result = _resolver.GetControllerTypes(assembliesResolver);

            return result;

        }
    }

    public class MyHttpControllerSelector : IHttpControllerSelector
    {
        private readonly IHttpControllerSelector _selector;

        public MyHttpControllerSelector(IHttpControllerSelector selector)
        {
            _selector = selector;
        }


        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var result = _selector.SelectController(request);

            return result;
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            var result = _selector.GetControllerMapping();

            return result;
        }
    }
}