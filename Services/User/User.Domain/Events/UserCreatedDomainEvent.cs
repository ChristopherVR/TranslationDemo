using User.Domain.AggregatesModel.UserAggregate;
using MediatR;

namespace User.Domain.Events;

public class UserCreatedDomainEvent : INotification
{
    public User.Domain.AggregatesModel.UserAggregate.User User { get; private set; }

    public UserCreatedDomainEvent(User.Domain.AggregatesModel.UserAggregate.User user) => User = user;
}
