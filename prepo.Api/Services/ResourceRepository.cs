using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;
using prepo.Api.Resources;

namespace prepo.Api.Services
{
    public class ResourceRepository
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public ResourceRepository(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public RootResource GetRootResource()
        {
            return new RootResource();
        }

        public UsersResource GetUserCollectionResource(int? page, int? count)
        {
            var resource = new UsersResource();

            if (count.HasValue && count.Value > 0)
            {
                var repo = _repositoryFactory.RepositoryFor<PrepoUser>();
                resource.Users = repo.GetMany(page ?? 1, count.Value);
            }

            return resource;
        }

        public UserResource GetUser(string id)
        {
            var resource = new UserResource(new PrepoUser(id));

            return resource;
        }

        public void SaveUser(string id, UserResource userResource)
        {
            var repo = _repositoryFactory.RepositoryFor<PrepoUser>();
            var user = userResource.Instance;

            if (id != null && id != user.Id)
            {
                throw new Exception("ID miss match");
            }

            repo.Put(user);
        }
    }
}