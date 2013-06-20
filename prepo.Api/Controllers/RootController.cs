using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override RootResource GetResourceCollection(int? page, int? count)
        {
            return _repository.GetRootResource();
        }
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
}