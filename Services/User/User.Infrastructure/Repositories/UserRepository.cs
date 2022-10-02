using Domain.Extensions.SeedWork;
using Microsoft.EntityFrameworkCore;
using User.Domain.AggregatesModel.UserAggregate;

namespace User.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public UserRepository(UserContext context) =>
        _context = context;

    public async Task<Domain.AggregatesModel.UserAggregate.User?> GetAsync(int entityId)
    {
        Domain.AggregatesModel.UserAggregate.User? entity = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Id == entityId);

        if (entity == null)
        {
            entity = _context
                .Users
                .Local
                .FirstOrDefault(u => u.Id == entityId);
        }

        if (entity is not null)
        {
            //await _context.Entry(entity)
            //    .Collection(r => r.)
            //    .LoadAsync();
        }

        return entity;
    }

    public Domain.AggregatesModel.UserAggregate.User Add(Domain.AggregatesModel.UserAggregate.User entity) => _context.Users.Add(entity).Entity;

    public void Update(Domain.AggregatesModel.UserAggregate.User entity) => _context.Entry(entity).State = EntityState.Modified;

    public async Task<Domain.AggregatesModel.UserAggregate.User?> GetAsync(string username)
    {
        Domain.AggregatesModel.UserAggregate.User? entity = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (entity == null)
        {
            entity = _context
                .Users
                .Local
                .FirstOrDefault(u => u.Username == username);
        }

        if (entity is not null)
        {
            //await _context.Entry(entity)
            //    .Collection(r => r.)
            //    .LoadAsync();
        }

        return entity;
    }

    public void Remove(Domain.AggregatesModel.UserAggregate.User entity)
        => _context.Remove(entity);
}
