using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;

namespace prepo.Api.Resources
{
    public class PersonaResource : HalItemResource<PrepoPersona>
    {
        public const string Self = RootResource.Self + "personas/";

        public PersonaResource(PrepoPersona persona)
            : base(Self + persona.Id, persona)
        { }
    }
}