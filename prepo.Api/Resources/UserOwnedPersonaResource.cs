using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class UserOwnedPersonaResource : HalItemResource<PrepoPersona>
    {
        private const string SelfPrefix = RootResource.Self + "personas/";

        public UserOwnedPersonaResource(PrepoPersona persona)
            : base(SelfPrefix + persona.Id, persona)
        { }
    }
}