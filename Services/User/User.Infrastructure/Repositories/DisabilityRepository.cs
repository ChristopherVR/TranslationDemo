using Domain.Extensions.SeedWork;
using Microsoft.EntityFrameworkCore;
using User.Domain.AggregatesModel.UserAggregate;

namespace User.Infrastructure.Repositories;
//public class DisabilityRepository : IDisabilityRepository
//{
//    private readonly UserContext _context;

//    public IUnitOfWork UnitOfWork => _context;

//    public DisabilityRepository(UserContext context) =>
//        _context = context;

//    public async Task<Domain.AggregatesModel.UserAggregate.Disability?> GetAsync(int entityId)
//    {
//        Domain.AggregatesModel.UserAggregate.Disability? entity = await _context
//            .Disabilities
//            .FirstOrDefaultAsync(u => u.Id == entityId);
       
//        if (entity == null)
//        {
//            entity = _context
//                .Disabilities
//                .Local
//                .FirstOrDefault(u => u.Id == entityId);
//        }
       
//        return entity;
//    }

//    public Domain.AggregatesModel.UserAggregate.Disability Add(Domain.AggregatesModel.UserAggregate.Disability entity) => _context.Disabilities.Add(entity).Entity;

//    public void Update(Domain.AggregatesModel.UserAggregate.Disability entity) => _context.Entry(entity).State = EntityState.Modified;

//    public void Remove(Domain.AggregatesModel.UserAggregate.Disability entity)
//        => _context.Remove(entity);
//}
