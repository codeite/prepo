using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure;
using prepo.Api.Models;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class ResourceController<TDbo> 
        : ApiController
        where TDbo : DbObject
    {
        private readonly IResourceResolutionService<TDbo> _resourceResolution;
        private IResourceRepository<TDbo> _resourceRepository;

        public ResourceController(IResourceResolutionService<TDbo> resourceResolution)
        {
            _resourceResolution = resourceResolution;
        }

        private IResourceRepository<TDbo> ResourceRepository
        {
            get
            {
                if (_resourceRepository == null)
                {
                    var resource = Request.GetRouteData().Values["resource"] as IHalResource;

                    if (resource == null)
                    {
                        throw new InvalidOperationException("resource should not be null");
                    }

                    _resourceRepository = _resourceResolution.GetRepositoryFor(resource);
                }

                return _resourceRepository;
            }

            set { _resourceRepository = value; }
        }

        public virtual HttpResponseMessage Get(int? page = null, int? count = null)
        {
            HttpResponseMessage response;

            ResourceRepository.Page = page;
            ResourceRepository.Count = count;

            if (ResourceRepository.ResourceExists())
            {
                response =  Request.CreateResponse(HttpStatusCode.OK, ResourceRepository.Resource);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return response;
        }

        public virtual HttpResponseMessage Put(TDbo content)
        {
            if (content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No content" };
            }

            var actionPerfomed = ResourceRepository.SaveResource(content);

            if (actionPerfomed == ActionPerfomed.NotSuported)
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }
            else
            {
                HttpStatusCode statusCode;
                switch (actionPerfomed)
                {
                    case ActionPerfomed.Created:
                        statusCode = HttpStatusCode.Created;
                        break;
                    case ActionPerfomed.Updated:
                        statusCode = HttpStatusCode.OK;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var response = Request.CreateResponse(statusCode, ResourceRepository.Resource);

                response.Headers.Location = new Uri(Request.RequestUri, ResourceRepository.ResourceLocation);
                return response;
            }
        }

        public virtual HttpResponseMessage Post(TDbo content)
        {
            if (content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No content" };
            }

            var actionPerfomed = ResourceRepository.SaveResource(content);

            if (actionPerfomed == ActionPerfomed.NotSuported)
            {
                return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
            }
            else
            {
                HttpStatusCode statusCode;
                switch (actionPerfomed)
                {
                    case ActionPerfomed.Created:
                        statusCode = HttpStatusCode.Created;
                        break;
                    case ActionPerfomed.Updated:
                        statusCode = HttpStatusCode.OK;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var response = Request.CreateResponse(statusCode, ResourceRepository.ChildResource(content.Id));

                response.Headers.Location = new Uri(Request.RequestUri, UriBuilderHelper.Combine(ResourceRepository.ResourceLocation, content.Id));
                return response;
            }
        }

        public virtual HttpResponseMessage Delete(IHalResource resource)
        {
            var repo = ResourceRepository;

            var actionPerfomed = repo.SaveResource(null);

            if (actionPerfomed == ActionPerfomed.Deleted)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }
    }
}