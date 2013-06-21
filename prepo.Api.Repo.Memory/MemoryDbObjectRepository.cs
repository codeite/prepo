using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;


namespace prepo.Api.Repo.Memory
{
    public class MemoryDbObjectRepository : IRepository<DbObject>
    {
        public DbObject GetOne(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DbObject> GetMany(int page, int count)
        {
            throw new NotImplementedException();
        }

        public void Put(DbObject item)
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
            MemoryRepository<PrepoUser>.Store.Clear();
        }

        public IQueryable<DbObject> Query()
        {
            throw new NotImplementedException();
        }
    }
}