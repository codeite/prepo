using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class UsersController : ResourceApiController<UsersResource, UserResource, PrepoUser>
    {
        private readonly UserResourceRepository _repository;

        public UsersController(UserResourceRepository repository)
        {
            _repository = repository;
        }

        protected override UsersResource GetResourceCollection(int? page, int? count)
        {
            return _repository.GetCollectionResource(page, count);
        }

        protected override UserResource GetResource(string id)
        {
            return _repository.GetById(id);
        }

        protected override string SaveResource(string id, UserResource content)
        {
            if (content == null)
            {
                _repository.Delete(id);
                return "";
            }
            else
            {
                _repository.SaveItem(id, content);

                return "/users/" + id;
            }
        }
    }
}