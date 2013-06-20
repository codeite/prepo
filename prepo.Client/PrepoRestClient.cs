using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ApiResource(Response response)
        {
            _response = response;
        }

        public string Body
        {
            get { return _response.Body; }
        }

        public ApiResource FollowRel(string users)
        {
            throw new NotImplementedException();
        }
    }
}
