using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;


namespace prepo.Api.Repo.Memory
{
    public class MemoryDbObjectRepository : IRepository<DbObject>
    {
        private static readonly Dictionary<Type, dynamic>  _userStores = new Dictionary<Type, dynamic>();

        public static Dictionary<string,T> StoreFor<T>()
        {
            var type = typeof (T);
            lock (_userStores)
            {
                if (!_userStores.ContainsKey(type))
                {
                    _userStores[type] = new Dictionary<string, T>();
                }

                return _userStores[type];
            }
        }

        public DbObject GetOne(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DbObject> GetMany(int page, int count)
        {
            throw new NotImplementedException();
        }

        public bool Put(DbObject item)
        {
            throw new NotImplementedException();
        }

        public string Post(DbObject item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            foreach (var userStore in _userStores.Values)
            {
                userStore.Clear();
            }
        }

        public IQueryable<DbObject> Query()
        {
            throw new NotImplementedException();
        }
    }
}