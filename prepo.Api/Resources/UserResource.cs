using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public static class UserResource
    {
        public const string CollectionName = "users";
        public const string ItemName = "user";

        public class Collection : HalPagedCollectionResource<PrepoUser>
        {
            public Collection(IHalResource owner)
                : base(owner, CollectionName, ItemName)
            {}

            protected override IItemResourceFactory<PrepoUser> ChildFactory
            {
                get { return new ItemResourceFactory<Item, PrepoUser>(); }
            }
        }

        public class Item : HalItemResource<PrepoUser>
        {
            public Item(string id, IHalResource owner)
                : base(id, owner, id)
            {}

            protected override Dictionary<string, IHalResource> ChildResources
            {
                get
                {
                    return new Dictionary<string, IHalResource>
                    {
                        {PersonaResource.CollectionName, new PersonaResource.Collection(this)}
                    };
                }
            }
        }
    }
}