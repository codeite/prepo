using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;

namespace prepo.Api.Services
{
    public class UserResourceRepository : ResourceRepository<UserCollectionResource, UserItemResource, PrepoUser>
    {
        public UserResourceRepository(IRepository<PrepoUser> repository)
            : base(repository)
        {
        }
    }
}