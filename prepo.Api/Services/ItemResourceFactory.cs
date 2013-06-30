using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure;
using prepo.Api.Resources.Base;

namespace prepo.Api.Services
{
    public class ItemResourceFactory<TItemResource, TDbo> : IItemResourceFactory<TDbo>
        where TDbo : DbObject
        where TItemResource : HalItemResource<TDbo>
    {
        public HalItemResource<TDbo> BuildResource(string id, IHalResource owner)
        {
            return GenericActivator.CreateInstance<TItemResource>(id, owner);
        }
    }
}