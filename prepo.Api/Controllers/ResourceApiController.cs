using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using prepo.Api.Resources;

namespace prepo.Api.Controllers
{
    public abstract class ResourceApiController<TCollection, TItem>
        : ResourceApiController<TCollection>
        where TCollection : HalResource
        where TItem : HalResource
    {
        protected virtual TItem GetResource(string id)
        {
            return null;
        }

        protected virtual string SaveResource(string id, TItem content)
        {
            return null;
        }

        public virtual HttpResponseMessage Get(string id)
        {
            var resource = GetResource(id);

            var response = Request.CreateResponse(resource != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                                                  resource);

            return response;
        }

        public virtual HttpResponseMessage Head(string id)
        {
            return Get(id);
        }

        public virtual HttpResponseMessage Put(string id, TItem content)
        {
            if (content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No content" };
            }

            var location = SaveResource(id, content);
            if (location == null)
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }
            else
            {
                var message = new HttpResponseMessage(HttpStatusCode.Created);
                message.Headers.Location = Request.RequestUri;
                return message;
            }
        }

        public virtual HttpResponseMessage Post(TItem content)
        {
            if (content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No content" };
            }

            var location = SaveResource(null, content);
            if (location == null)
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }
            else
            {
                var message = new HttpResponseMessage(HttpStatusCode.Created);
                message.Headers.Location = new Uri(Request.RequestUri, content.SelfLink.Href);
                return message;
            }
        }

        public virtual HttpResponseMessage Delete()
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }

    public abstract class ResourceApiController<TCollection> : ApiController
        where TCollection : HalResource
    {
        protected virtual TCollection GetResourceCollection(int? page, int? count)
        {
            return null;
        }

        public virtual HttpResponseMessage Get(int? page = null, int? count = null)
        {
            var resource = GetResourceCollection(page, count);

            var response = Request.CreateResponse(resource != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                                                  resource);

            return response;
        }

        public virtual HttpResponseMessage Head()
        {
            return new HttpResponseMessage();
        }

    }
}