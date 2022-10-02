using User.Domain.AggregatesModel.UserAggregate;
using User.Infrastructure.Repositories;
using Module = Autofac.Module;

namespace User.API.Infrastructure.AutofacModules;

public class RepositoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserRepository>()
            .As<IUserRepository>()
            .InstancePerLifetimeScope();
    }
}
