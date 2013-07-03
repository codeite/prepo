using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Models;
using prepo.Api.Services;

namespace prepo.Api.Resources.Base
{
    public abstract class HalSingleResource<TDbo> : HalResource<TDbo>
        where TDbo : DbObject
    {
        protected HalSingleResource(IHalResource owner, string resourceName)
            : base(owner, resourceName)
        {
        }

        protected abstract Dictionary<string, IHalResource> ChildResources { get; }

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

    public abstract class HalItemResource<TDbo> : HalSingleResource<TDbo>
        where TDbo : DbObject
    {
        protected HalItemResource(string id, IHalResource owner, string resourceName)
            : base(owner, resourceName)
        {
            Id = id;
        }

        protected override void AddProperties(Dictionary<string, object> dictionary, DbObject instance)
        {

            if (instance != null)
            {
                var type = instance.GetType();

                foreach (var propertyInfo in type.GetProperties())
                {
                    var name = FixName(propertyInfo.Name);
                    var value = propertyInfo.GetValue(instance);

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

        public string Id { get; set; }

        

        /*
        public override SaveResourceResult<DbObject> SaveResource(DbObject content)
        {
            if (content == null)
            {
                Repository.Delete(Id);
                return new SaveResourceResult<DbObject>(SaveResourceResult<DbObject>.ActionPerfomedOptions.Deleted);
            }
            else
            {
                string id = Id;
                var updated = Repository.SaveItem(ref id, content);
                return new SaveResourceResult<DbObject>(updated ? SaveResourceResult<DbObject>.ActionPerfomedOptions.Updated : SaveResourceResult<DbObject>.ActionPerfomedOptions.Created)
                {
                    Location = SelfLink.Href,
                    Resource = Repository.GetById(Id),
                };
            }
        } 
        */
    }
}