using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public abstract class HalResource
    {
        private readonly ResourceLink _self;

        protected HalResource(string selfHref)
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

    public abstract class HalResource<T> 
        : HalResource
        where T : DbObject
    {
        private readonly T _instance;

        protected HalResource(string selfHref, T instance) : base(selfHref)
        {
            _instance = instance;
        }

        public T Instance
        {
            get { return _instance; }
        }

        protected override void AddProperties(Dictionary<string, object> dictionary)
        {
            var type = typeof (T);

            foreach (var propertyInfo in type.GetProperties())
            {
                var name = FixName(propertyInfo.Name);
                var value = propertyInfo.GetValue(_instance);

                dictionary.Add(name, value);
            }
        }

        private string FixName(string name)
        {
            if (name.Length > 0)
            {
                var first = name[0].ToString(CultureInfo.InvariantCulture).ToLower()[0];

                name = first + name.Substring(1);
            }

            return name;
        }
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