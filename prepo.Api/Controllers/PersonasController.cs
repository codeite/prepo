using prepo.Api.Contracts.Models;
using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class PersonasController : DefaultController<PersonaCollectionResource, PersonaItemResource, PrepoPersona>
    {
        public PersonasController(PersonaResourceRepository repository)
            : base(repository)
        {
        }
    }
}