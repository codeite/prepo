using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Services
{
    public class UserRepository : IUserRepository
    {

        public PrepoUser GetOne(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PrepoUser> GetMany(int? page, int? count)
        {
            throw new NotImplementedException();
        }

        public void Put(PrepoUser item)
        {
            throw new NotImplementedException();
        }

        public string Post(PrepoUser item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable Query()
        {
            throw new NotImplementedException();
        }
    }
}