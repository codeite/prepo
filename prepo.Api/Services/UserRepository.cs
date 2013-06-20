using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Services
{
    public class UserRepository : MemoryRepository<PrepoUser>, IUserRepository
    {

    }

    public class MemoryRepository<T> where T : DbObject
    {
        private static readonly Dictionary<string, T> _store = new Dictionary<string, T>(); 

        public T GetOne(string id)
        {
            return _store[id];
        }

        public IEnumerable<T> GetMany(int page, int count)
        {
            var start = (page - 1)*count;
            return _store.Values.Skip(start).Take(count).ToList();
        }

        public void Put(T item)
        {
            _store[item.Id] = item;
        }

        public string Post(T item)
        {
            item.Id = (_store.Keys.Select(int.Parse).Max() + 1).ToString();

            _store[item.Id] = item;

            return item.Id;
        }

        public void Delete(string id)
        {
            _store.Remove(id);
        }

        public IQueryable Query()
        {
            return _store.AsQueryable();
        }
    }
}