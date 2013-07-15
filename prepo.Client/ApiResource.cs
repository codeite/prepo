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

        public dynamic BodyAsJson()
        {
            return DynamicJsonObject.ReadJson(_response.Body);
        }

        public Dictionary<string, dynamic> Json
        {
            get { return _json.Value; }
        }

        public Response Response
        {
            get { return _response; }
        }

        public ApiResource FollowRel(string name)
        {
            return FollowRel(name, (object) null);
        }

        public ApiResource FollowRel(string name, object arguments = null)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            return new ApiResource(_response.Get(href));
        }

        public ApiResource FollowRelTemplate(string name, Dictionary<string, string> arguments = null)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            return new ApiResource(_response.Get(href));
        }

        public ApiResource FollowRelList(string[] linkRelList)
        {
            var resource = this;

            foreach (var linkRel in linkRelList)
            {
                if (linkRel.Contains(":"))
                {
                    var parts = linkRel.Split(':');

                    Dictionary<string, string> dict = parts.Skip(1).Select(x => x.Split('=')).ToDictionary(k => k[0], v => v[1]);
                    resource = resource.FollowRelTemplate(parts.First(), dict);
                }
                else
                {
                    resource = resource.FollowRel(linkRel);
                }
            }

            return resource;
        }

        public ApiResource PutToRel(string name, object arguments, BodyContent content)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            var response = _response.Put(href, content);

            return new ApiResource(response);
        }

        public Response PostToRel(string name, object arguments, BodyContent content)
        {
            var linkHrefTemplate = Json["_links"][name]["href"] as string;

            var href = ResolveTemplate(linkHrefTemplate, arguments);

            return _response.Post(href, content);
        }

        public Response Post(BodyContent content)
        {
            return _response.Post("", content);
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
            if (arguments == null)
            {
                return linkHrefTemplate;
            }

            var args = ToDictionary(arguments);

            return ResolveTemplate(linkHrefTemplate, args);
        }

        private string ResolveTemplate(string linkHrefTemplate, IDictionary<string, string> args)
        {
            if (args == null)
            {
                return linkHrefTemplate;
            }

            var template = new UriTemplate(linkHrefTemplate);

            foreach (var arg in args)
            {
                template.SetParameter(arg.Key, arg.Value);
            }

            var href = template.Resolve();
            return href;
        }
    }
}