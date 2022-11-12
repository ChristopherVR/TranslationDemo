using Domain.Extensions.SeedWork;

namespace User.Domain.AggregatesModel.UserAggregate;
public class UserDisability : Entity, IAggregateRoot
{
    public string UpdatedUser { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    public int DisabilityId { get; private set; }
    public bool Disabled { get; private set; }

    public UserDisability(
        string updatedUser,
        int disabilityId,
        bool disabled)
    {
        UpdatedDate = DateTime.UtcNow;
        UpdatedUser = updatedUser;
        DisabilityId = disabilityId;
        Disabled = disabled;
    }
}
