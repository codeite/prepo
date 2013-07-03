using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;

namespace prepo.Api.Services
{
    public interface IItemResourceFactory<TDbo> 
        where TDbo : DbObject
    {
        HalItemResource<TDbo> BuildResource(string id, IHalResource owner);
    }
}