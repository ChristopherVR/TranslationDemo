﻿using API.Extensions.Application.Behaviors;
using Module = Autofac.Module;

namespace User.API.Infrastructure.AutofacModules;
public class MediatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        builder.Register<ServiceFactory>(context =>
        {
            var c = context.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });

        builder.RegisterAssemblyTypes(GetType().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

        builder.RegisterAssemblyTypes(GetType().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));

        builder.RegisterAssemblyTypes(GetType().Assembly)
            .AsClosedTypesOf(typeof(IValidator<>));

        builder.RegisterGeneric(typeof(LoggingBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));

        builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
    }
}
