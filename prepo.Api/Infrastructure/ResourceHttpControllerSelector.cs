// Type: System.Web.Http.Dispatcher.DefaultHttpControllerSelector
// Assembly: System.Web.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Projects\prepo\prepo.Api\bin\System.Web.Http.dll

extern alias sys_web_http;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using prepo.Api.Controllers;
using foo = sys_web_http::System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Properties;
using System.Web.Http.Routing;
using System.Web.Http.Dispatcher;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Infrastructure
{
    /*
    public class MyDefaultHttpControllerSelector : IHttpControllerSelector
    {
        public static readonly string ControllerSuffix = "Controller";
        private const string ControllerKey = "controller";
        private readonly HttpConfiguration _configuration;
        //private readonly HttpControllerTypeCache _controllerTypeCache;
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;
        private dynamic SRResources;

        static MyDefaultHttpControllerSelector()
        {
        }

        public MyDefaultHttpControllerSelector(HttpConfiguration configuration)
        {
            if (configuration == null)
                throw Error.ArgumentNull("configuration");
            this._controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(new Func<ConcurrentDictionary<string, HttpControllerDescriptor>>(this.InitializeControllerInfoCache));
            this._configuration = configuration;
        }

        public virtual HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw Error.ArgumentNull("request");
            }
        

            string controllerName = this.GetControllerName(request);

            if (string.IsNullOrEmpty(controllerName))
            {
                throw new Exception(Error.Format("no controller in route"));
            }

            HttpControllerDescriptor controllerDescriptor;
            if (this._controllerInfoCache.Value.TryGetValue(controllerName, out controllerDescriptor))
            {
                return controllerDescriptor;
            }

            ICollection<Type> controllerTypes = this._controllerTypeCache.GetControllerTypes(controllerName);
            if (controllerTypes.Count != 0)
                throw DefaultHttpControllerSelector.CreateAmbiguousControllerException(HttpRequestMessageExtensions.GetRouteData(request).Route, controllerName, controllerTypes);
            throw new HttpResponseException(HttpRequestMessageExtensions.CreateErrorResponse(request, HttpStatusCode.NotFound, Error.Format(SRResources.ResourceNotFound, new object[1]
            {
                (object) request.RequestUri
            }), Error.Format(SRResources.DefaultControllerFactory_ControllerNameNotFound, new object[1]
            {
                (object) controllerName
            })));
        }

        public virtual IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return (IDictionary<string, HttpControllerDescriptor>)Enumerable.ToDictionary<KeyValuePair<string, HttpControllerDescriptor>, string, HttpControllerDescriptor>((IEnumerable<KeyValuePair<string, HttpControllerDescriptor>>)this._controllerInfoCache.Value, (Func<KeyValuePair<string, HttpControllerDescriptor>, string>)(c => c.Key), (Func<KeyValuePair<string, HttpControllerDescriptor>, HttpControllerDescriptor>)(c => c.Value), (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
        }

        public virtual string GetControllerName(HttpRequestMessage request)
        {
            if (request == null)
                throw Error.ArgumentNull("request");
            IHttpRouteData routeData = HttpRequestMessageExtensions.GetRouteData(request);
            if (routeData == null)
                return (string)null;
            string str = (string)null;
            DictionaryExtensions.TryGetValue<string>(routeData.Values, "controller", out str);
            return str;
        }

        private static Exception CreateAmbiguousControllerException(IHttpRoute route, string controllerName, ICollection<Type> matchingTypes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Type type in (IEnumerable<Type>)matchingTypes)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(type.FullName);
            }
            return (Exception)new InvalidOperationException(Error.Format(SRResources.DefaultControllerFactory_ControllerNameAmbiguous_WithRouteTemplate, (object)controllerName, (object)route.RouteTemplate, (object)stringBuilder));
        }

        private ConcurrentDictionary<string, HttpControllerDescriptor> InitializeControllerInfoCache()
        {
            ConcurrentDictionary<string, HttpControllerDescriptor> concurrentDictionary = new ConcurrentDictionary<string, HttpControllerDescriptor>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
            HashSet<string> hashSet = new HashSet<string>();
            foreach (KeyValuePair<string, ILookup<string, Type>> keyValuePair in this._controllerTypeCache.Cache)
            {
                string key = keyValuePair.Key;
                foreach (IEnumerable<Type> enumerable in (IEnumerable<IGrouping<string, Type>>)keyValuePair.Value)
                {
                    foreach (Type controllerType in enumerable)
                    {
                        if (concurrentDictionary.Keys.Contains(key))
                        {
                            hashSet.Add(key);
                            break;
                        }
                        else
                            concurrentDictionary.TryAdd(key, new HttpControllerDescriptor(this._configuration, key, controllerType));
                    }
                }
            }
            foreach (string key in hashSet)
            {
                HttpControllerDescriptor controllerDescriptor;
                concurrentDictionary.TryRemove(key, out controllerDescriptor);
            }
            return concurrentDictionary;
        }
    }

    public class Error
    {
        public static string Format(string format, params object[] args)
        {
            throw new Exception(format + string.Join(", ", args));
        }

        public static Exception ArgumentNull(string name)
        {
            throw new Exception("Arg Null: "+name);
        }
    }
    */

    public class ResourceHttpControllerSelector : IHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private readonly IHttpControllerSelector _fallback;
        private readonly ResourceRepositoryFactory _resourceRepositoryFactory;
        private readonly ResourceChainBuilder _resourceChainBuilder;

        public ResourceHttpControllerSelector(HttpConfiguration configuration, IHttpControllerSelector fallback, ResourceRepositoryFactory resourceRepositoryFactory)
        {
            _configuration = configuration;
            _fallback = fallback;
            _resourceRepositoryFactory = resourceRepositoryFactory;
            _resourceChainBuilder = new ResourceChainBuilder();
        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var path = GetPath(request);

            var resource = GetResource(path);

            SetResource(request, resource);

            string name = "Resource+" + resource.DboType.Name;
            var type = typeof(ResourceController<>).MakeGenericType(resource.DboType);

            return new HttpControllerDescriptor(_configuration, name, type);
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            throw new NotImplementedException();
        }

        public virtual string GetPath(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            IHttpRouteData routeData = foo.HttpRequestMessageExtensions.GetRouteData(request);

            if (routeData == null)
            {
                return null;
            }

            var resourcepath = "path";
            if (!routeData.Values.ContainsKey(resourcepath))
            {
                return null;
            }

            return routeData.Values[resourcepath].ToString();

        }

        public virtual void SetResource(HttpRequestMessage request, IHalResource resource)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            var routeData = request.GetRouteData();

            if (routeData == null)
            {
                throw new InvalidOperationException("Could not get route data");
            }

            routeData.Values["resource"] = resource;

        }

        private IHalResource GetResource(string path)
        {
            var rootResource = _resourceChainBuilder.Build(path);
            return rootResource.Head;
        }
    }
}
