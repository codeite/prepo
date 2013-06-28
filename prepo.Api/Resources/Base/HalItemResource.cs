using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources.Base
{
    public abstract class HalItemResource<TDbo> : HalResource
        where TDbo : DbObject
    {
        private readonly TDbo _instance;

        protected HalItemResource(string id, IHalResource owner, string resourceName)
            : base(owner, resourceName)
        {
            Id = id;
        }

        public TDbo Instance
        {
            get
            {
                return null; // _instance.Value; 
            }
        }

        protected override void AddProperties(Dictionary<string, object> dictionary)
        {
            if (_instance != null)
            {
                var type = typeof (TDbo);

                foreach (var propertyInfo in type.GetProperties())
                {
                    var name = FixName(propertyInfo.Name);
                    var value = propertyInfo.GetValue(_instance);

                    dictionary.Add(name, value);
                }
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

        protected abstract Dictionary<string, IHalResource> ChildResources { get; }

        public string Id { get; set; }

        public override void ReadChildResources(Stack<string> partsStack)
        {
            if (partsStack.Any())
            {
                var directChild = partsStack.Pop().ToLowerInvariant();

                if (!ChildResources.ContainsKey(directChild))
                {
                    throw new Exception("Unknown child resource: " + directChild);
                }

                Child = ChildResources[directChild];

                Child.ReadChildResources(partsStack);
            }
        }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            return ChildResources.Select(kvp => new ResourceLink(kvp.Key, kvp.Value));
        }
    }
}