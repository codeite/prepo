using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure;
using prepo.Api.Resources.Base;

namespace prepo.Api.Resources
{
    public class PersonaCollectionResource : PagedCollectionResource<PrepoPersona>
    {
        public PersonaCollectionResource(string location)
            : base(UriBuilderHelper.Combine(location, "/personas"), "persona", "personas")
        {
        }
    }
}