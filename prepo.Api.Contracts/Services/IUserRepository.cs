using prepo.Api.Contracts.Models;

namespace prepo.Api.Contracts.Services
{
    public interface IUserRepository : IRepository<PrepoUser>
    {
    }

    public interface IDbObjectRepository : IRepository<DbObject>
    {
        
    }
}