using User.API.Application.Queries;
using Module = Autofac.Module;

namespace User.API.Infrastructure.AutofacModules;

public class QueryModule : Module
{
    private readonly string _queryConnectionString;
    public QueryModule(string queryConnectionString)
    {
        _queryConnectionString = queryConnectionString;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new UserQueries(_queryConnectionString))
            .As<IUserQueries>()
            .InstancePerLifetimeScope();
    }
}

