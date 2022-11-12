using User.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace User.Domain.Events;

public class UserCreatedDomainEvent : INotification
{
    public User.Domain.AggregatesModel.UserAggregate.Disability User { get; private set; }

    public UserCreatedDomainEvent(User.Domain.AggregatesModel.UserAggregate.Disability user) => User = user;
}
