using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class UserCollectionResource : HalCollectionResource<PrepoUser>
    {
        public const string Self = RootResource.Self + "users";

        public UserCollectionResource()
            : base(Self)
        { }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("user", Self + "/{id}");
            yield return new ResourceLink("first", Self+"?page=1&count=10");
            yield return new ResourceLink("page", Self+"?page={page}&count={count}");

            if (Items != null)
            {
                foreach (var user in Items)
                {
                    yield return new ResourceLink("users", Self+"/"+user.Id);
                }
            }
        }
    }
}