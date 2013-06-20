using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class UsersResource : HalResource
    {
        public const string Self = RootResource.Self + "users";
        public UsersResource()
            : base(Self)
        { }

        public IEnumerable<PrepoUser> Users { get; set; }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("user", Self + "/{id}");
            yield return new ResourceLink("first", Self+"?page=1&count=10");
            yield return new ResourceLink("page", Self+"?page={page}&count={count}");

            if (Users != null)
            {
                foreach (var user in Users)
                {
                    yield return new ResourceLink("users", Self+"/"+user.Id);
                }
            }
        }
    }
}