using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Repo.Memory
{
    public class MemoryRepository<T> where T : DbObject
    {
        public static readonly Dictionary<string, T> Store = new Dictionary<string, T>();

        public T GetOne(string id)
        {
            T value;
            Store.TryGetValue(id, out value);
            return value;
        }

        public IEnumerable<T> GetMany(int page, int count)
        {
            var start = (page - 1) * count;
            return Store.Values.Skip(start).Take(count).ToList();
        }

        public void Put(T item)
        {
            Store[item.Id] = item;
        }

        public string Post(T item)
        {
            item.Id = (Store.Keys.Select(int.Parse).Max() + 1).ToString();

            Store[item.Id] = item;

            return item.Id;
        }

        public void Delete(string id)
        {
            Store.Remove(id);
        }

        public void DeleteAll()
        {
            Store.Clear();
        }

        public IQueryable<T> Query()
        {
            return Store.Values.AsQueryable();
        }
    }
}