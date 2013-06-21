using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prepo.Api.Resources
{
    public abstract class HalCollectionResource<TDbo> : HalResource
    {
        private readonly ResourceLink _self;

        public IEnumerable<TDbo> Items { get; set; }

        protected HalCollectionResource(string selfHref)
        {
            _self = new ResourceLink("self", selfHref);
        }

        public ResourceLink SelfLink
        {
            get { return _self; }
        }

        public object ToDynamicJson()
        {
            var root = new Dictionary<string, object>();
            var links = new Dictionary<string, object>();

            links[_self.Name] = MakeHref(_self.Href);
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

        public virtual IEnumerable<HalResource> GetEmbededResources()
        {
            yield break;
        }

        private Dictionary<string, object> MakeHref(string value)
        {
            return new Dictionary<string, object> {{"href", value}};
        }
    }

    public interface HalResource
    {
        object ToDynamicJson();
    }

    /*
    public class PersonaResource : HalResource
    {
       
    }

    public class AddressesResource : HalResource
    {

    }

    public class CurrentAddressResource : HalResource
    {

    }
    */
}