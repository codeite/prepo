using prepo.Api.Contracts.Models;

namespace prepo.Api.Services
{
    public interface IDboRepository
    {
        void Delete(string id);
        bool SaveItem(ref string id, DbObject content);
        DbObject GetById(string id);
        void DeleteAll();
    }
}