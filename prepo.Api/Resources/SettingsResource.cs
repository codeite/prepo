using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public static class SettingsResource
    {
        public const string CollectionName = "settings";
        public const string ItemName = "setting";

        public class Collection : HalPagedCollectionResource<PrepoSetting>
        {
            public Collection(IHalResource owner)
                : base(owner, CollectionName, ItemName)
            { }

            protected override IItemResourceFactory<PrepoSetting> ChildFactory
            {
                get { return new ItemResourceFactory<Item, PrepoSetting>(); }
            }
        }

        public class Item : HalItemResource<PrepoSetting>
        {
            public Item(string id, IHalResource owner)
                : base(id, owner, id)
            {
            }

            protected override Dictionary<string, IHalResource> ChildResources
            {
                get
                {
                    return new Dictionary<string, IHalResource>
                    {
          
                    };
                }
            }
        }
    }
}