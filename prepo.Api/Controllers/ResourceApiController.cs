using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources;

namespace prepo.Api.Controllers
{
    public abstract class ResourceApiController<TCollection, TItem, TDbo>
        : ResourceApiController<TCollection, TDbo>
        where TCollection : HalCollectionResource<TDbo>
        where TItem : HalCollectionResource<TDbo>
        where TDbo : DbObject
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

        public virtual HttpResponseMessage Delete(string id)
        {
            var location = SaveResource(id, null);

            if (location == null)
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }

    public abstract class ResourceApiController<TCollection, TDbo> : ApiController
        where TCollection : HalCollectionResource<TDbo>
        where TDbo : DbObject
    {
        protected virtual TCollection GetResourceCollection(int? page, int? count)
        {
            return null;
        }

        protected virtual bool DeleteResourceCollection()
        {
            return false;
        }

        public virtual HttpResponseMessage Get(int? page = null, int? count = null)
        {
            var resource = GetResourceCollection(page, count);

            var response = Request.CreateResponse(resource != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                                                  resource);

            return response;
        }

        public virtual HttpResponseMessage Head(int? page = null, int? count = null)
        {
            return Get(page, count);
        }

        public virtual HttpResponseMessage Delete()
        {
            return DeleteResourceCollection() ? 
                new HttpResponseMessage(HttpStatusCode.OK) : 
                new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }
    }
}