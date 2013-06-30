using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources.Base
{
    public interface IHalResourceInstance
    {
        object ToDynamicJson();
    }

    public class HalCollectionResourceInstance : IHalResourceInstance
    {
        private readonly IHalResource _resource;

        public HalCollectionResourceInstance(IHalResource resource)
        {
            _resource = resource;
        }

        public IEnumerable<DbObject> Items { get; set; }
        public int? Page { get; set; }
        public int? Count { get; set; }

        public IEnumerable<ResourceLink> GetAdditionalRelatedResources()
        {
            var baseLink = _resource.SelfLink.Href;

            yield return new ResourceLink("page", baseLink + "?page={page}&count={count}");
            yield return new ResourceLink("first", baseLink + "?page=1&count=10");

            if (Page.HasValue)
            {
                yield return new ResourceLink("next", string.Format("{0}?page={1}&count={2}", baseLink, Page + 1, Count));

                if (Page.Value > 1)
                {
                    yield return new ResourceLink("prev", string.Format("{0}?page={1}&count={2}", baseLink, (Page - 1), Count));
                }
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    yield return new ResourceLink(_resource.ResourceName, baseLink + "/" + item.Id);
                }
            }
        }
    

        public object ToDynamicJson()
        {
            return _resource.ToDynamicJson(null, GetAdditionalRelatedResources());
        }
    }

    public class HalInstanceResourceInstance : IHalResourceInstance
    {
         private readonly IHalResource _resource;
        private readonly DbObject _instance;

        public HalInstanceResourceInstance(IHalResource resource, DbObject instance)
        {
            _resource = resource;
            _instance = instance;
        }

        public object ToDynamicJson()
        {
            return _resource.ToDynamicJson(_instance, GetAdditionalRelatedResources());
        }

        public IEnumerable<ResourceLink> GetAdditionalRelatedResources()
        {
            yield break;
        }
    }
}