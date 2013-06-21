﻿using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Repo.Memory
{
    public class MemoryRepository<T> : IRepository<T>
        where T : DbObject
    {
        private static readonly Dictionary<string, T> _store = MemoryDbObjectRepository.StoreFor<T>();

        public T GetOne(string id)
        {
            T value;
            _store.TryGetValue(id, out value);
            return value;
        }

        public IEnumerable<T> GetMany(int page, int count)
        {
            var start = (page - 1) * count;
            return _store.Values.Skip(start).Take(count).ToList();
        }

        public bool Put(T item)
        {
            var exists = _store.ContainsKey(item.Id);
            _store[item.Id] = item;
            return exists;
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

        public void DeleteAll()
        {
            _store.Clear();
        }

        public IQueryable<T> Query()
        {
            return _store.Values.AsQueryable();
        }
    }
}