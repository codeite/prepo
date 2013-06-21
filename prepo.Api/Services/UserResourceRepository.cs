using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;

namespace prepo.Api.Services
{
    public class UserResourceRepository : ResourceRepository<UsersResource, UserResource, PrepoUser>
    {
        public UserResourceRepository(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
        }
    }
}