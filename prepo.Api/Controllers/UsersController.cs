using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class UsersController : ResourceApiController<UsersResource>
    {
        private readonly ResourceRepository _repository;

        public UsersController(ResourceRepository repository)
        {
            _repository = repository;
        }


        protected override UsersResource GetResource(int? page, int? count)
        {
            return _repository.GetUserCollectionResource(page, count);
        }
    }
}