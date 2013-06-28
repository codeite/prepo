using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public class RootResource : HalItemResource<DbObject>
    {
        public RootResource()
            : base("", null, "")
        {
        }

        protected override Dictionary<string, IHalResource> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource>
                {
                    { UserCollectionResource.CollectionName, new UserCollectionResource(this) },
                    { PersonaCollectionResource.CollectionName, new PersonaCollectionResource(this) }
                };
            }
        }
    }
}