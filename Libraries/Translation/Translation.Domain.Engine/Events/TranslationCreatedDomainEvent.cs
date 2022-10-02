using MediatR;
using Translation.Domain.Engine.SeedWork;

namespace Translation.Domain.Engine.Events;

public class TranslationCreatedDomainEvent : INotification
{
    public LocalizedEntity Entity { get; private set; }

    public TranslationCreatedDomainEvent(LocalizedEntity entity) => Entity = entity;
}
