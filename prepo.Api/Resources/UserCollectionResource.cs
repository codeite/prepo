using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class UserCollectionResource : PagedCollectionResource<PrepoUser>
    {
        public const string Self = RootResource.Self + "users";

        public UserCollectionResource()
            : base(Self, "user", "users")
        {
        }
    }
}