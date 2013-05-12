using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class RootController : ResourceApiController<RootResource>
    {
        private readonly ResourceRepository _repository;

        public RootController(ResourceRepository repository)
        {
            _repository = repository;
        }

        protected override RootResource GetResource()
        {
            return _repository.GetRootResource();
        }
    }

    public class UserController : ResourceApiController<UserResource>
    {

    }
    /*
    public class PersonaController : ResourceApiController<PersonaResource>
    {

    }

    public class AddressesController : ResourceApiController<AddressesResource>
    {
        
    }

    public class CurrentAddressController : ResourceApiController<CurrentAddressResource>
    {
        
    }
    */
    public abstract class ResourceApiController<T> : ApiController where T : HalResource
    {
        protected virtual T GetResource()
        {
            return null;
        }

        public virtual HttpResponseMessage Get()
        {
            var resource = GetResource();

            var response =  Request.CreateResponse(resource != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                                          resource);

            return response;
        }

        public virtual HttpResponseMessage Head()
        {
            return new HttpResponseMessage();
        }

        public virtual HttpResponseMessage Get(string id)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public virtual HttpResponseMessage Head(string id)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public virtual HttpResponseMessage Put(string id)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public virtual HttpResponseMessage Post()
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public virtual HttpResponseMessage Delete()
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}