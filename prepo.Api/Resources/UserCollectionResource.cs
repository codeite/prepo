using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure;

namespace prepo.Api.Resources
{
    public class UserCollectionResource : PagedCollectionResource<PrepoUser>
    {
        public UserCollectionResource(string location)
            : base(UriBuilderHelper.Combine(location, "users"), "user", "users")
        {
        }
    }
}