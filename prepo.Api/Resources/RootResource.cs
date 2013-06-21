using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class RootResource : HalCollectionResource<DbObject>
    {
        public const string Self = "/";
        public RootResource()
            : base(Self)
        { }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("users", UserCollectionResource.Self);
            yield return new ResourceLink("personas", PersonaCollectionResource.Self);
        }
    }
}