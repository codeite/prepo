using Autofac;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Repo.Memory
{
    public static class MemoryRepoRegisterIoc
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryUserRepository>().As<IUserRepository>();
            builder.RegisterType<MemoryDbObjectRepository>().As<IDbObjectRepository>();
        }
    }
}
