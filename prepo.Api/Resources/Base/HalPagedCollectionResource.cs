using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Resources.Base
{
    public abstract class HalPagedCollectionResource<TDbo>
        : HalCollectionResource<TDbo>
        where TDbo : DbObject
    {
        private readonly string _itemCollectionName;

        /*
        public PagedCollectionResource(string self, string itemTemlateName, string itemCollectionName)
            : base(self)
        {
            _itemTemlateName = itemTemlateName;
            _itemCollectionName = itemCollectionName;
        }
        */

        protected HalPagedCollectionResource(IHalResource owner, string resourceName, string itemResourceName)
            : base(owner, resourceName, itemResourceName)
        {
        }



    }
}