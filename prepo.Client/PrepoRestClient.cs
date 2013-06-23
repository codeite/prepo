using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everest;
using Everest.Headers;
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

            options = options.Concat(new []{new Accept("application/json"), }).ToArray();

            _client = new RestClient(baseUri, options);    
        }

        public ApiResource GetRoot()
        {
            return new ApiResource(_client.Get("/"));
        }
    }
}
