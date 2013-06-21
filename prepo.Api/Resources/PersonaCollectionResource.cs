using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class PersonaCollectionResource : PagedCollectionResource<PrepoPersona>
    {
        public const string Self = RootResource.Self + "personas";

        public PersonaCollectionResource()
            : base(Self, "persona", "personas")
        {
        }
    }
}