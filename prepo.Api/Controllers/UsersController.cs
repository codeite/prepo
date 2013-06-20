using System;
using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class UsersController : ResourceApiController<UsersResource, UserResource>
    {
        private readonly ResourceRepository _repository;

        public UsersController(ResourceRepository repository)
        {
            _repository = repository;
        }

        protected override UsersResource GetResourceCollection(int? page, int? count)
        {
            return _repository.GetUserCollectionResource(page, count);
        }

        protected override UserResource GetResource(string id)
        {
            return _repository.GetUser(id);
        }

        protected override string SaveResource(string id, UserResource content)
        {
            _repository.SaveUser(id, content);

            return "/users/" + id;
        }

        public override System.Net.Http.HttpResponseMessage Get(int? page = null, int? count = null)
        {
            return base.Get(page, count);
        }

        public override System.Net.Http.HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }

        public override System.Net.Http.HttpResponseMessage Post(UserResource content)
        {
            return base.Post(content);
        }
    }
}