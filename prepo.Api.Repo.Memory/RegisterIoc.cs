using Autofac;
using prepo.Api.Contracts.Models;
using prepo.Api.Contracts.Services;

namespace prepo.Api.Repo.Memory
{
    public static class MemoryRepoRegisterIoc
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryDbObjectRepository>().As<IRepository<DbObject>>();
            builder.RegisterType<MemoryRepository<PrepoUser>>().As<IRepository<PrepoUser>>();
            builder.RegisterType<MemoryRepository<PrepoPersona>>().As<IRepository<PrepoPersona>>();
        }
    }
}
