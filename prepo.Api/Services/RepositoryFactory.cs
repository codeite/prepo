using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Services
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IDbObjectRepository> _dbobjectRepository;

        public RepositoryFactory(Lazy<IUserRepository> userRepository, Lazy<IDbObjectRepository> dbobjectRepository)
        {
            _userRepository = userRepository;
            _dbobjectRepository = dbobjectRepository;
        }

        public IRepository<T> RepositoryFor<T>()
        {
            if (typeof(T) == typeof(PrepoUser))
            {
                return (IRepository<T>)_userRepository.Value;
            }

            if (typeof(T) == typeof(DbObject))
            {
                return (IRepository<T>)_dbobjectRepository.Value;
            }

            throw new NotSupportedException("Do not have a repository for: "+typeof(T));
        }
    }
}