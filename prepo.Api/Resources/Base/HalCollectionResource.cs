using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Infrastructure;
using prepo.Api.Models;
using prepo.Api.Services;

namespace prepo.Api.Resources.Base
{
    public abstract class HalCollectionResource<TDbo>
        : HalResource<TDbo>
        where TDbo : DbObject
    {
        private readonly string _itemResourceName;

        protected HalCollectionResource(IHalResource owner, string resourceName, string itemResourceName)
            : base(owner, resourceName)
        {
            _itemResourceName = itemResourceName;
        }

        //public IEnumerable<TDbo> Items { get; set; }

        protected abstract IItemResourceFactory<TDbo> ChildFactory { get; }


        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink(_itemResourceName, SelfLink.Href + "/{id}");
        }

        public override void ReadChildResources(Stack<string> partsStack)
        {
            if (partsStack.Any())
            {
                var id = partsStack.Pop();

                Child = ChildFactory.BuildResource(id, this);

                Child.ReadChildResources(partsStack);
            }
        }

        /*
        public override SaveResourceResult<DbObject> SaveResource(DbObject content)
        {
            if (content == null)
            {
                Repository.DeleteAll();
                return new SaveResourceResult<DbObject>(SaveResourceResult<DbObject>.ActionPerfomedOptions.Deleted);
            }
            else
            {
                string id = null;
                var updated = Repository.SaveItem(ref id, content);
                return new SaveResourceResult<DbObject>(updated ? SaveResourceResult<DbObject>.ActionPerfomedOptions.Updated : SaveResourceResult<DbObject>.ActionPerfomedOptions.Created)
                {
                    Location = UriBuilderHelper.Combine(SelfLink.Href, id),
                    Resource = Repository.GetById(id),
                };
            }
        }
        */

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