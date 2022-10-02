using Domain.Extensions.SeedWork;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace User.Infrastructure;

public class UserContext : DbContext, IUnitOfWork
{
    public const string DefaultSchema = "Config";

    public required DbSet<Domain.AggregatesModel.UserAggregate.User> Users { get; set; }

    private readonly IMediator _mediator;

    public UserContext(DbContextOptions<UserContext> options, IMediator mediator) : base(options) =>
        _mediator = mediator;

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);

        await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}

