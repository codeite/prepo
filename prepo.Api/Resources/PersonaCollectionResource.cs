using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public class PersonaCollectionResource : HalPagedCollectionResource<PrepoPersona>
    {
        public const string CollectionName = "personas";

        public PersonaCollectionResource(IHalResource owner)
            : base(owner, CollectionName, PersonaItemResource.ItemName)
        {
        }

        protected override IItemResourceFactory<PrepoPersona> ChildFactory
        {
            get { return new ItemResourceFactory<PersonaItemResource, PrepoPersona>(); }
        }
    }
}