using System.Collections;
using System.Collections.Generic;
using prepo.Api.Infrastructure;

namespace prepo.Api.Resources.Base
{
    public abstract class HalResource : IHalResource
    {
        private readonly string _resourceName;
        private readonly ResourceLink _selfLink;

        protected HalResource(IHalResource owner, string resourceName)
        {
            _resourceName = resourceName;
            var urlToSelf = owner == null ? "/" : UriBuilderHelper.Combine(owner.SelfLink.Href, resourceName);
            _selfLink = new ResourceLink("self", urlToSelf);
            Owner = owner;
        }

        public ResourceLink SelfLink
        {
            get { return _selfLink; }
        }

        public string ResourceName
        {
            get { return _resourceName; }
        }

        public virtual object ToDynamicJson()
        {
            var root = new Dictionary<string, object>();
            var links = new Dictionary<string, object>();

            links[_selfLink.Name] = MakeHref(_selfLink.Href);
            foreach (var relatedResource in GetRelatedResources())
            {
                var href = MakeHref(relatedResource.Href);

                if (links.ContainsKey(relatedResource.Name))
                {
                    var item = links[relatedResource.Name];

                    if (item is IList)
                    {
                        (item as IList).Add(href);
                    }
                    else
                    {
                        links[relatedResource.Name] = new List<object> { item, href };
                    }
                }
                else
                {
                    links[relatedResource.Name] = href;
                }
            }
            root["_links"] = links;
            AddProperties(root);
            GetEmbededResources();
            return root;
        }

        protected virtual void AddProperties(Dictionary<string, object> dictionary)
        {
            
        }

        public virtual IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield break;
        }

        public virtual IEnumerable<IHalResource> GetEmbededResources()
        {
            yield break;
        }

        public abstract void ReadChildResources(Stack<string> partsStack);
        public virtual IHalResource Child { get; protected set; }
        public virtual IHalResource Head { get { return Child == null ? this : Child.Head; } }
        public virtual IHalResource Owner { get; private set; }

        private Dictionary<string, object> MakeHref(string value)
        {
            return new Dictionary<string, object> {{"href", value}};
        }
    }
}