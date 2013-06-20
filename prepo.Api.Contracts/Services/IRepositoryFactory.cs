namespace prepo.Api.Contracts.Services
{
    public interface IRepositoryFactory
    {
        IRepository<T> RepositoryFor<T>();
    }
}