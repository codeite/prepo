using System;
using Autofac;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Services
{
    public class ResourceRepositoryFactory
    {
        private readonly IComponentContext _container;

        public ResourceRepositoryFactory(IComponentContext container)
        {
            _container = container;
        }

        public IDboRepository For(Type dboType)
        {
            var repoType = typeof (IRepository<>).MakeGenericType(dboType);
            dynamic repo = _container.Resolve(repoType);
            return new DboRepository(repo);
        }
    }

    public class DboRepository : IDboRepository
    {
        private readonly dynamic _repository;

        public DboRepository(dynamic repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
        }

        public void Delete(string id)
        {
           _repository.Delete(id);
        }

        public bool SaveItem(ref string id, DbObject content)
        {
            if (id == null)
            {
                id = _repository.Post(content);
                return false;
            }
            else
            {
                return _repository.Put(content);
            }
        }

        public DbObject GetById(string id)
        {
            return _repository.GetOne(id);
        }

        public void DeleteAll()
        {
           _repository.DeleteAll();
        }
    }
}