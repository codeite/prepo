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
        private readonly IRepository<TDbo> _repository;

        public ResourceRepository(IRepository<TDbo> repository)
        {
            _repository = repository;
        }

        public TCollectionResource GetResource()
        {
            return GetCollectionResource(null, null);
        }

        public TCollectionResource GetCollectionResource(int? page, int? count)
        {
            var resource = CreateResource(page, count);

            if (page.HasValue && page.Value > 0)
            {
                resource.Page = page.Value;
                resource.Count = count ?? 10;
                resource.Items = _repository.GetMany(page.Value, count ?? 10);
            }

            return resource;
        }

        private TCollectionResource CreateResource(int? page, int? count)
        {
            return Activator.CreateInstance<TCollectionResource>();
        }

        public TItemResource GetById(string id)
        {
            var item = _repository.GetOne(id);

            if (item != null)
            {
                return Activator.CreateInstance(typeof(TItemResource), item) as TItemResource;
            }

            return null;
        }

        public bool SaveItem(string id, TItemResource userItemResource)
        {
            var user = userItemResource.Instance;

            if (id != null && id != user.Id)
            {
                throw new Exception("ID miss match");
            }

            return _repository.Put(user);
        }

        public void DeleteAll()
        {
            _repository.DeleteAll();
        }

        public void Delete(string id)
        {
            _repository.Delete(id);
        }
    }
}