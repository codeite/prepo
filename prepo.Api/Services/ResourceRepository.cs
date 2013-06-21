using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;

namespace prepo.Api.Services
{
    public class ResourceRepository<TCollectionResource, TItemResource, TDbo>
        where TCollectionResource : HalCollectionResource<TDbo>
        where TItemResource : HalItemResource<TDbo>
        where TDbo : DbObject
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public ResourceRepository(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public TCollectionResource GetResource()
        {
            return GetCollectionResource(null, null);
        }

        public TCollectionResource GetCollectionResource(int? page, int? count)
        {
            var resource = CreateResource();

            if (count.HasValue && count.Value > 0)
            {
                var repo = _repositoryFactory.RepositoryFor<TDbo>();
                resource.Items = repo.GetMany(page ?? 1, count.Value);
            }

            return resource;
        }

        private TCollectionResource CreateResource()
        {
            return Activator.CreateInstance<TCollectionResource>();
        }

        public TItemResource GetById(string id)
        {
            var repo = _repositoryFactory.RepositoryFor<TDbo>();

            var item = repo.GetOne(id);

            if (item != null)
            {
                return Activator.CreateInstance(typeof(TItemResource), item) as TItemResource;
            }

            return null;
        }

        public void SaveItem(string id, TItemResource userItemResource)
        {
            var repo = _repositoryFactory.RepositoryFor<TDbo>();
            var user = userItemResource.Instance;

            if (id != null && id != user.Id)
            {
                throw new Exception("ID miss match");
            }

            repo.Put(user);
        }

        public void DeleteAll()
        {
            _repositoryFactory.RepositoryFor<TDbo>().DeleteAll();
        }

        public void Delete(string id)
        {
            var repo = _repositoryFactory.RepositoryFor<TDbo>();
            repo.Delete(id);
        }
    }
}