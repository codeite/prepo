using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources.Base
{
    public abstract class HalPagedCollectionResource<T> : HalCollectionResource<T>
        where T : DbObject
    {
        private readonly string _itemCollectionName;

        /*
        public PagedCollectionResource(string self, string itemTemlateName, string itemCollectionName)
            : base(self)
        {
            _itemTemlateName = itemTemlateName;
            _itemCollectionName = itemCollectionName;
        }
        */

        protected HalPagedCollectionResource(IHalResource owner, string resourceName, string itemResourceName)
            : base(owner, resourceName, itemResourceName)
        {
        }

        public int? Page { get; set; }

        public int Count { get; set; }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            foreach (var relatedResource in base.GetRelatedResources())
            {
                yield return relatedResource;
            }

            yield return new ResourceLink("page", SelfLink.Href + "?page={page}&count={count}");
            yield return new ResourceLink("first", SelfLink.Href + "?page=1&count=10");

            if (Page.HasValue)
            {
                yield return new ResourceLink("next", string.Format("{0}?page={1}&count={2}", SelfLink.Href, Page + 1, Count));

                if (Page.Value > 1)
                {
                    yield return new ResourceLink("prev", string.Format("{0}?page={1}&count={2}", SelfLink.Href, (Page - 1), Count));
                }
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    yield return new ResourceLink(ResourceName, SelfLink.Href + "/" + item.Id);
                }
            }
        }
    }
}