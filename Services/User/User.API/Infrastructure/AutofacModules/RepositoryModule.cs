﻿using Module = Autofac.Module;

namespace User.API.Infrastructure.AutofacModules;

public class RepositoryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<UserRepository>()
        //    .As<IUserRepository>()
        //    .InstancePerLifetimeScope();

        //builder.RegisterType<DisabilityRepository>()
        //    .As<IDisabilityRepository>()
        //    .InstancePerLifetimeScope();
    }
}
