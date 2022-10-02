using Domain.Extensions.SeedWork;

namespace User.Domain.AggregatesModel.UserAggregate;
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetAsync(int id);
    User Add(User entity);
    void Update(User entity);
    void Remove(User entity);
}
