using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public static class PersonaResource
    {
        public const string CollectionName = "personas";
        public const string ItemName = "persona";

        public class Collection : HalPagedCollectionResource<PrepoPersona>
        {
            public Collection(IHalResource owner)
                : base(owner, CollectionName, ItemName)
            {}

            protected override IItemResourceFactory<PrepoPersona> ChildFactory
            {
                get { return new ItemResourceFactory<Item, PrepoPersona>(); }
            }
        }

        public class Item : HalItemResource<PrepoPersona>
        {
            public Item(string id, IHalResource owner)
                : base(id, owner, id)
            {}

            protected override Dictionary<string, IHalResource> ChildResources
            {
                get
                {
                    return new Dictionary<string, IHalResource>
                    {};
                }
            }
        }
    }
}