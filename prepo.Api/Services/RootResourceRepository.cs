using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;

namespace prepo.Api.Services
{
    public class RootResourceRepository : ResourceRepository<RootResource, HalItemResource<DbObject>, DbObject> 
    {
        public RootResourceRepository(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}