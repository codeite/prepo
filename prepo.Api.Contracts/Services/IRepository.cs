using System.Collections.Generic;
using System.Linq;

namespace prepo.Api.Contracts.Services
{
    public interface IRepository<T>
    {
        bool Exists(string id);
        T GetOne(string id);
        IEnumerable<T> GetMany(int page, int count);
        bool Put(T item);
        string Post(T item);
        void Delete(string id);
        void DeleteAll();
        IQueryable<T> Query();
    }
}