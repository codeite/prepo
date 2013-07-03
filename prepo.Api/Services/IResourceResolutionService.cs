using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources.Base;

namespace prepo.Api.Services
{
    public interface IResourceResolutionService<TDbo> where TDbo : DbObject
    {
        IResourceRepository<TDbo> GetRepositoryFor(IHalResource resource);
    }

    class ResourceResolutionService<TDbo>
        : IResourceResolutionService<TDbo>
        where TDbo : DbObject
    {
        private readonly Lazy<IRepository<TDbo>> _repository;

        public ResourceResolutionService(Lazy<IRepository<TDbo>> repository)
        {
            _repository = repository;
        }

        public IResourceRepository<TDbo> GetRepositoryFor(IHalResource resource)
        {
            if (resource is HalItemResource<TDbo>)
            {
                return new ItemResourceRepository<TDbo>(_repository.Value, resource as HalItemResource<TDbo>);
            }
            else if (resource is HalSingleResource<TDbo>)
            {
                return new SingleResourceRepository<TDbo>(_repository.Value, resource as HalSingleResource<TDbo>);
            }
            else if (resource is HalCollectionResource<TDbo>)
            {
                return new CollectionResourceRepository<TDbo>(_repository.Value, resource as HalCollectionResource<TDbo>);
            }

            throw new Exception("Unknown repo type: " + resource.GetType());
        }
    }
}