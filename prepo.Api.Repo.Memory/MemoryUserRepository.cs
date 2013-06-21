using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Repo.Memory
{
    public class MemoryUserRepository : MemoryRepository<PrepoUser>, IUserRepository
    {

    }
}