using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public abstract IEnumerable<ResourceLink> GetRelatedResources();

        public object ToDynamicJson()
        {
            var root = new Dictionary<string, object>();
            var links = new Dictionary<string, object>();

            links[_self.Name] = MakeHref(_self.Href);
            foreach (var relatedResource in GetRelatedResources())
            {
                links[relatedResource.Name] = MakeHref(relatedResource.Href);
            }
            root["_links"] = links;
            AddProperties(root);
            GetEmbededResources();
            return root;
        }

        public virtual void AddProperties(Dictionary<string, object> dictionary)
        {
            
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

    public class ResourceLink
    {
        public ResourceLink(string name, string href, string title = null)
        {
            Name = name;
            Href = href;
            Title = title;
        }

        public string Name { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
    }

    public class RootResource : HalResource
    {
        public const string Self = "/";
        public RootResource()
            : base(Self)
        { }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("users", UserResource.Self);
        }
    }

    public class UserResource : HalResource
    {
        public const string Self = RootResource.Self + "users";
        public UserResource()
            : base(Self)
        { }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield break;
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