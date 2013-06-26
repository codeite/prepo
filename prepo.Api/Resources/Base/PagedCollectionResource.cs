using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources.Base
{
    public class PagedCollectionResource<T> 
        : HalCollectionResource<T>
        where T : DbObject
    {
        private readonly string _itemTemlateName;
        private readonly string _itemCollectionName;

        public PagedCollectionResource(string self, string itemTemlateName, string itemCollectionName)
            : base(self)
        {
            _itemTemlateName = itemTemlateName;
            _itemCollectionName = itemCollectionName;
        }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink(_itemTemlateName, _self.Href + "/{id}");
            yield return new ResourceLink("page", _self.Href + "?page={page}&count={count}");
            yield return new ResourceLink("first", _self.Href + "?page=1&count=10");

            if (Page.HasValue)
            {
                yield return new ResourceLink("next", string.Format("{0}?page={1}&count={2}", _self.Href, Page + 1, Count));

                if (Page.Value > 1)
                {
                    yield return new ResourceLink("prev", string.Format("{0}?page={1}&count={2}", _self.Href, (Page - 1), Count));
                }
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    yield return new ResourceLink(_itemCollectionName, _self.Href + "/" + item.Id);
                }
            }
        }
    }
}