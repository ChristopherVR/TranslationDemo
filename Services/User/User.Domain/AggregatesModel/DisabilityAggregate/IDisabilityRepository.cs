using Domain.Extensions.SeedWork;

namespace User.Domain.AggregatesModel.UserAggregate;
public interface IDisabilityRepository : IRepository<Disability>
{
    Task<Disability?> GetAsync(int id);
    Disability Add(Disability entity);
    void Update(Disability entity);
    void Remove(Disability entity);
}
