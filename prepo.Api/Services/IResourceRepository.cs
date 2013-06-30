using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Infrastructure;
using prepo.Api.Models;
using prepo.Api.Resources.Base;

namespace prepo.Api.Services
{
    public interface IResourceRepository<in TDbo>
        where TDbo : DbObject
    {
        bool ResourceExists();
        ActionPerfomed SaveResource(TDbo newContent);
        IHalResourceInstance Resource { get; }
        IHalResourceInstance ChildResource(string id);
        int? Page { get; set; }
        int? Count { get; set; }
        string ResourceLocation { get; }
    }

    public abstract class ResourceRepository<TDbo>
        : IResourceRepository<TDbo>
        where TDbo : DbObject
    {
        private readonly IHalResource _resource;

        protected ResourceRepository(IHalResource resource)
        {
            _resource = resource;
        }

        public abstract bool ResourceExists();
        public abstract ActionPerfomed SaveResource(TDbo newContent);
        public abstract IHalResourceInstance Resource { get; }
        public abstract IHalResourceInstance ChildResource(string id);

        public int? Page { get; set; }
        public int? Count { get; set; }
        public string ResourceLocation { get { return _resource.RebuildPath(); } }
    }

    class CollectionResourceRepository<TDbo> : ResourceRepository<TDbo> where TDbo : DbObject
    {
        private readonly IRepository<TDbo> _repository;
        private readonly HalCollectionResource<TDbo> _resource;

        public CollectionResourceRepository(IRepository<TDbo> repository, HalCollectionResource<TDbo> resource)
            :base(resource)
        {
            _repository = repository;
            _resource = resource;
        }

        public override bool ResourceExists()
        {
            return true;
        }

        public override ActionPerfomed SaveResource(TDbo newContent)
        {
            if (newContent == null)
            {
                _repository.DeleteAll();
                return ActionPerfomed.Deleted;
            }
            else
            {
                _repository.Post(newContent);
                return ActionPerfomed.Created;
            }
        }

        public override IHalResourceInstance Resource
        {
            get
            {
                var collection = new HalCollectionResourceInstance(_resource);

                if (Page.HasValue && Page.Value > 0)
                {
                    if (_resource is HalPagedCollectionResource<TDbo>)
                    {
                        var count = Count ?? 10;
                        collection.Page = Page.Value;
                        collection.Count = count;
                        collection.Items = _repository.GetMany(Page.Value, count);
                    }
                    else
                    {
                        collection.Items = _repository.GetMany(1, int.MaxValue);
                    }
                }

                return collection;
            }
        }

        public override IHalResourceInstance ChildResource(string id)
        {
            var stack = new Stack<string>();
            stack.Push(id);
            _resource.ReadChildResources(stack);

            return new HalInstanceResourceInstance(_resource.Child, _repository.GetOne(id));
        }
    }

    class SingleResourceRepository<TDbo> : ResourceRepository<TDbo> where TDbo : DbObject
    {
        private readonly IRepository<TDbo> _repository;
        private readonly HalSingleResource<TDbo> _resource;

        public SingleResourceRepository(IRepository<TDbo> repository, HalSingleResource<TDbo> resource) : base(resource)
        {
            _repository = repository;
            _resource = resource;
        }

        public override bool ResourceExists()
        {
            return true;
        }

        public override ActionPerfomed SaveResource(TDbo newContent)
        {
            if (newContent == null)
            {
                _repository.DeleteAll();
                return ActionPerfomed.Deleted;
            }

            return ActionPerfomed.NotSuported;
        }

        public override IHalResourceInstance Resource { get { return new HalInstanceResourceInstance(_resource, null); } }
        public override IHalResourceInstance ChildResource(string id)
        {
            throw new NotImplementedException();
        }
    }

    class ItemResourceRepository<TDbo> : ResourceRepository<TDbo> where TDbo : DbObject
    {
        private readonly IRepository<TDbo> _repository;
        private readonly HalItemResource<TDbo> _resource;
        private readonly Lazy<TDbo> _instance;

        public ItemResourceRepository(IRepository<TDbo> repository, HalItemResource<TDbo> resource) : base(resource)
        {
            _repository = repository;
            _resource = resource;
            _instance = new Lazy<TDbo>(() => _repository.GetOne(_resource.Id));
        }

        public override bool ResourceExists()
        {
            return _repository.Exists(_resource.Id);
        }

        public override ActionPerfomed SaveResource(TDbo newContent)
        {
            if (newContent == null)
            {
                _repository.Delete(_resource.Id);
                return ActionPerfomed.Deleted;
            }
            else
            {
                if (newContent.Id != _resource.Id)
                {
                    throw new Exception("ID Mismatch");
                }

                var updated = _repository.Put(newContent);
                return updated ? ActionPerfomed.Updated : ActionPerfomed.Created;
            }
        }

        public override IHalResourceInstance Resource { get { return new HalInstanceResourceInstance(_resource, _instance.Value); } }
        public override IHalResourceInstance ChildResource(string id)
        {
            throw new NotImplementedException();
        }
    }
}