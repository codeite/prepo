using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Infrastructure;
using prepo.Api.Models;
using prepo.Api.Services;

namespace prepo.Api.Resources.Base
{
    public abstract class HalResource<TDbo>
        : IHalResource
        where TDbo : DbObject
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

        public Type DboType { get { return typeof (TDbo); }}

        public string RebuildPath()
        {
            if (Owner != null)
            {
                return UriBuilderHelper.Combine(Owner.RebuildPath(), ResourceName);
            }
            else
            {
                return ResourceName;
            }
        }

        public virtual string ToXml(DbObject instance, IEnumerable<ResourceLink> additionalLinks = null)
        {
            var document = new XDocument();


            var root = new XElement("resource");
            document.Add(root);
            root.Add(new XAttribute("href", _selfLink.Href));

            foreach (var relatedResource in GetRelatedResources().Concat(additionalLinks ?? new ResourceLink[0]))
            {
                root.Add(new XElement("link", new XAttribute("href", relatedResource.Href), new XAttribute("rel", relatedResource.Name)));
            }


            return document.ToString();
        }

        public virtual object ToDynamicJson(DbObject instance, IEnumerable<ResourceLink> additionalLinks = null)
        {
            var root = new Dictionary<string, object>();
            var links = new Dictionary<string, object>();

            links[_selfLink.Name] = MakeHref(_selfLink.Href);
            foreach (var relatedResource in GetRelatedResources().Concat(additionalLinks ?? new ResourceLink[0]))
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
            AddProperties(root, instance);
            GetEmbededResources();
            return root;
        }

        protected virtual void AddProperties(Dictionary<string, object> dictionary, DbObject dbObject)
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