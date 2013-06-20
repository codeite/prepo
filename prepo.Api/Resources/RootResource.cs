using System.Collections.Generic;

namespace prepo.Api.Resources
{
    public class RootResource : HalResource
    {
        public const string Self = "/";
        public RootResource()
            : base(Self)
        { }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("users", UsersResource.Self);
        }
    }
}