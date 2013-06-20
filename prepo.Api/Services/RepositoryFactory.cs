using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Services
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly Lazy<IUserRepository> _userRepository;

        public RepositoryFactory(Lazy<IUserRepository> userRepository)
        {
            _userRepository = userRepository;
        }

        public IRepository<T> RepositoryFor<T>()
        {
            if (typeof(T) == typeof(PrepoUser))
            {
                return (IRepository<T>)_userRepository.Value;
            }

            throw new NotSupportedException("Do not have a repository for: "+typeof(T));
        }
    }
}