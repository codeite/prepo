using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;


namespace prepo.Api.Repo.Memory
{
    public class RootMemoryRepository : IRepository<PrepoRoot>
    {
        private static readonly Dictionary<Type, dynamic>  _userStores = new Dictionary<Type, dynamic>();
        public static readonly PrepoRoot Root = new PrepoRoot();

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

        public bool Exists(string id)
        {
            return true;
        }

        public PrepoRoot GetOne(string id)
        {
            return Root;
        }

        public IEnumerable<PrepoRoot> GetMany(int page, int count)
        {
            yield return Root;
        }

        public bool Put(PrepoRoot item)
        {
            throw new NotImplementedException();
        }

        public string Post(PrepoRoot item)
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

        public IQueryable<PrepoRoot> Query()
        {
            throw new NotImplementedException();
        }
    }
}