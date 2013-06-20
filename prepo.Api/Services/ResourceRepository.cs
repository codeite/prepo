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
                resource.Users = repo.GetMany(page, count);
            }

            return resource;
        }
    }
}