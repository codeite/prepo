using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;

namespace prepo.Api.Resources
{
    public class PersonaItemResource : HalItemResource<PrepoPersona>
    {
        public const string ItemName = "persona";
        /*
        public const string Self = RootResource.Self + "personas/";

        public PersonaResource(PrepoPersona persona)
            : base(Self + persona.Id, persona)
        { }
        */

        public PersonaItemResource(string id, IHalResource owner)
            : base(id, owner, id)
        {
        }

        protected override Dictionary<string, IHalResource> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource>
                {
                };
            }
        }
    }
}