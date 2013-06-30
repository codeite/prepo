using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;
using prepo.Api.Resources.Base;

namespace prepo.Api.Services
{
    public class OldResourceRepository<TCollectionResource, TItemResource, TDbo>
        where TCollectionResource : HalResource<TDbo>
        where TItemResource : HalItemResource<TDbo>
        where TDbo : DbObject
    {
        private readonly Contracts.Services.IRepository<TDbo> _repository;

        public OldResourceRepository(Contracts.Services.IRepository<TDbo> repository)
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
                if (resource is HalPagedCollectionResource<TDbo>)
                {
                    var pagedResource = resource as HalPagedCollectionResource<TDbo>;

                    //pagedResource.Page = page.Value;
                    //pagedResource.Count = count ?? 10;
                    //pagedResource.Items = _repository.GetMany(page.Value, count ?? 10);
                }
                else if (resource is HalCollectionResource<TDbo>)
                {
                    var collectionResource = resource as HalCollectionResource<TDbo>;
                    //collectionResource.Items = _repository.GetMany(1, int.MaxValue);
                }
            }

            return resource;
        }

        private TCollectionResource CreateResource(int? page, int? count)
        {
            return Activator.CreateInstance(typeof(TCollectionResource), "/") as TCollectionResource;
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

        public bool SaveItem(ref string id, TItemResource userItemResource)
        {
            throw new NotImplementedException();
            /*
            var instance = userItemResource.Instance;

            if (id != null && id != instance.Id)
            {
                throw new Exception("ID miss match");
            }

            id = instance.Id;

            return _repository.Put(instance);
             */
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