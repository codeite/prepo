using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public class UserItemResource : HalItemResource<PrepoUser>
    {
        public const string ItemName = "user";

        public UserItemResource(string id, IHalResource owner)
            : base(id, owner, id)
        {
        }

        protected override Dictionary<string, IHalResource> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource>
                {
                    {PersonaCollectionResource.CollectionName, new PersonaCollectionResource(this)}
                };
            }
        }
    }
}