using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everest;
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

        public Response GetRoot()
        {
            return _client.Get("/");
        }
    }
}
