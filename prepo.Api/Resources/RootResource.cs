using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;

namespace prepo.Api.Resources
{
    public class RootResource : HalCollectionResource<DbObject>
    {
        public const string Self = "/";
        public RootResource(string owner)
            : base(Self)
        { }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("users", new UserCollectionResource(Self));
            yield return new ResourceLink("personas", new PersonaCollectionResource(Self));
        }
    }
}