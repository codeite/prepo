using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public class PersonaItemResource : HalItemResource<PrepoPersona>
    {
        public const string ItemName = "persona";

        public PersonaItemResource( string id, IHalResource owner)
            : base(id, owner, id)
        {
        }

        protected override Dictionary<string, IHalResource> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource>
                {
                };
            }
        }
    }
}