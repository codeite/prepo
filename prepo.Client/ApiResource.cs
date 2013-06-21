using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Codeite.Core.Json;
using Everest;
using Everest.Content;

namespace prepo.Client
{
    public class ApiResource 
    {
        private readonly Response _response;
        private readonly Lazy<Dictionary<string, dynamic>> _json;

        public ApiResource(Response response)
        {
            _response = response;
            _json = new Lazy<Dictionary<string, dynamic>>(() => DynamicJsonObject.ReadJson(Body));
        }

        public string Body
        {
            get { return _response.Body; }
        }

        public Dictionary<string, dynamic> Json
        {
            get { return _json.Value; }
        }

        public ApiResource FollowRel(string name)
        {
            //var linkHref = ("$._links." + name + ".href");
            var linkHref = Json["_links"][name]["href"] as string;

            return new ApiResource(_response.Get(linkHref));
        }

        public ApiResource FollowRel(string name, object arguments)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            return new ApiResource(_response.Get(href));
        }

        public string PutToRel(string name, object arguments, BodyContent content)
        {
            Response nullResponse;
            return PutToRel(name, arguments, content, out nullResponse);
        }

        public string PutToRel(string name, object arguments, BodyContent content, out Response response)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            response = _response.Put(href, content);

            return response.Location;
        }

        public string PostToRel(string name, object arguments, BodyContent content)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            return _response.Post(href, content).Location;
        }

        public string Post(BodyContent content)
        {
            return _response.Post("", content).Location;
        }

        public void Delete()
        {
            _response.Delete("");
        }

        public bool DeleteRel(string name, object arguments)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            return _response.Delete(href).StatusCode == HttpStatusCode.OK;
        }

        private IDictionary<string, string> ToDictionary(object arguments)
        {
            if (arguments == null)
            {
                return new Dictionary<string, string>(); 
            }

            var type = arguments.GetType();

            return type.GetProperties()
                       .ToDictionary(
                           p => p.Name, 
                           p => p.GetValue(arguments).ToString());

        }

        private string ResolveTemplate(string linkHrefTemplate, object arguments)
        {
            var template = new UriTemplate(linkHrefTemplate);

            var args = ToDictionary(arguments);

            foreach (var arg in args)
            {
                template.SetParameter(arg.Key, arg.Value);
            }

            var href = template.Resolve();
            return href;
        }
    }
}