using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;
using prepo.Api.Resources.Base;

namespace prepo.Api.Services
{
    public class RootResourceRepository : ResourceRepository<RootResource, HalItemResource<DbObject>, DbObject> 
    {
        public RootResourceRepository(Contracts.Services.IRepository<DbObject> repository) 
            : base(repository)
        {
        }
    }
}