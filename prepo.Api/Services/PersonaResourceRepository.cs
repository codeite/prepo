using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;

namespace prepo.Api.Services
{
    public class PersonaResourceRepository : ResourceRepository<PersonaCollectionResource, PersonaResource, PrepoPersona>
    {
        public PersonaResourceRepository(IRepository<PrepoPersona> repository)
            : base(repository)
        {
        }
    }
}