using MasterDataEngine.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Translation.API.Engine.Application.Commands;
internal sealed class CreateTranslationCommandHandler : IRequestHandler<CreateTranslationCommand, Unit>
{
    private readonly ILogger<CreateTranslationCommandHandler> _logger;
    private readonly ITranslationRepository _translationRepository;

    public CreateTranslationCommandHandler(ILogger<CreateTranslationCommandHandler> logger, ITranslationRepository translationRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
    }

    public async Task<Unit> Handle(CreateTranslationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new translation {request}", request);

        await _translationRepository.CreateTranslationAsync(request.UpdatedBy, request.TableName, request.RecordId, request.Culture, request.Value, cancellationToken);

        return Unit.Value;
    }
}
