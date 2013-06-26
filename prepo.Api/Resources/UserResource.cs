using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;

namespace prepo.Api.Resources
{
    public class UserResource : HalItemResource<PrepoUser>
    {
        public UserResource(PrepoUser user)
            : base(RootResource.Self + "users/" + user.Id, user)
        { }

        public override System.Collections.Generic.IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("personas", new PersonaCollectionResource(_self.Href));
        }
    }
}