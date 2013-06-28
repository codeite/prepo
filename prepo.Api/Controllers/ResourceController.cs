using System.Net;
using System.Net.Http;
using System.Web.Http;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class ResourceController : ApiController
    {
        private readonly ResourceChainBuilder _resourceChainBuilder;

        public ResourceController(ResourceChainBuilder resourceChainBuilder)
        {
            _resourceChainBuilder = resourceChainBuilder;
        }

        //public virtual HttpResponseMessage Get()
        //{
        //    return Get("");
        //}

        public virtual HttpResponseMessage Get(string path)
        {
            var resource = GetResource(path);
           
            var response = Request.CreateResponse(resource != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                                                  resource);
            return response;
        }

        public virtual HttpResponseMessage Delete(string path)
        {
            //var resource = GetResource(path);

            //var repository = ResourceRespositoryFactory.GetRepositoryFor

            //var location = SaveResource(id, null);

            //if (location == null)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            //}
            //else
            //{
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            //}
        }

        private IHalResource GetResource(string path)
        {
            var rootResource = _resourceChainBuilder.Build(path);

            var resource = rootResource.Head;

            return resource;
        }

        /*
        private SaveResourceResult SaveResource(string id, IHalResource content)
        {
            if (content == null)
            {
                _repository.Delete(id);
                return new SaveResourceResult<TItem>(SaveResourceResult<TItem>.ActionPerfomedOptions.Deleted);
            }
            else
            {
                var updated = _repository.SaveItem(ref id, content);
                return new SaveResourceResult<TItem>(updated ? SaveResourceResult<TItem>.ActionPerfomedOptions.Updated : SaveResourceResult<TItem>.ActionPerfomedOptions.Created)
                {
                    Location = content.SelfLink.Href,
                    Resource = _repository.GetById(id),
                };
            }
        }
        */
    }
}