
namespace User.API.Application.DomainEventHandlers.UserCreated;

public class CreateSomeTranslationExampleDomainEventHandler : INotificationHandler<Translation.Domain.Engine.Events.TranslationCreatedDomainEvent>
{
    private readonly ILogger<CreateSomeTranslationExampleDomainEventHandler> _logger;
    public CreateSomeTranslationExampleDomainEventHandler(ILogger<CreateSomeTranslationExampleDomainEventHandler> logger)
        => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Handle(Translation.Domain.Engine.Events.TranslationCreatedDomainEvent translationCreatedDomainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("The translation created domain event handler triggered. {Entity}", translationCreatedDomainEvent.Entity);
        await Task.CompletedTask;
    }
}
