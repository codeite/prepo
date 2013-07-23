using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public class RootResource : HalSingleResource<PrepoRoot>
    {
        public RootResource()
            : base(null, "")
        {
        }

        protected override Dictionary<string, IHalResource> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource>
                {
                    { UserResource.CollectionName, new UserResource.Collection(this) },
                    { PersonaResource.CollectionName, new PersonaResource.Collection(this) },
                    { SettingsResource.CollectionName, new SettingsResource.Collection(this) },
                };
            }
        }
    }
}