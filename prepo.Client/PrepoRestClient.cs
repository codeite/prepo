using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeite.Core.Json;
using Everest;
using Everest.Content;
using Everest.Pipeline;

namespace prepo.Client
{
    public class PrepoRestClient
    {
        private readonly Resource _client;

        public PrepoRestClient(string baseUri = null, params PipelineOption[] options)
        {
            if (baseUri == null)
            {
                baseUri = "http://dev.prepo.codeite.com";
            }

            _client = new RestClient(baseUri, options);    
        }

        public ApiResource GetRoot()
        {
            return new ApiResource(_client.Get("/"));
        }
    }

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
    }
}
