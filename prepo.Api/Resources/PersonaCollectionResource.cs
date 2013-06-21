using System.Collections.Generic;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class PersonaCollectionResource : HalCollectionResource<PrepoPersona>
    {
        public const string Self = RootResource.Self + "personas";

        public PersonaCollectionResource()
            : base(Self)
        { }

        public override IEnumerable<ResourceLink> GetRelatedResources()
        {
            yield return new ResourceLink("persona", Self + "/{id}");
            yield return new ResourceLink("first", Self + "?page=1&count=10");
            yield return new ResourceLink("page", Self + "?page={page}&count={count}");

            if (Items != null)
            {
                foreach (var persona in Items)
                {
                    yield return new ResourceLink("personas", Self + "/" + persona.Id);
                }
            }
        }
    }
}