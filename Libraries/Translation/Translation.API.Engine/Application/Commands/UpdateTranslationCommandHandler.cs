using MediatR;
using Microsoft.Extensions.Logging;
using Translation.Infrastructure.Repositories;

namespace Translation.API.Engine.Application.Commands;
internal sealed class UpdateTranslationCommandHandler : IRequestHandler<UpdateTranslationCommand, Unit>
{
    private readonly ILogger<UpdateTranslationCommandHandler> _logger;
    private readonly ITranslationRepository _translationRepository;

    public UpdateTranslationCommandHandler(ILogger<UpdateTranslationCommandHandler> logger, ITranslationRepository translationRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
    }

    public async Task<Unit> Handle(UpdateTranslationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating translation {request}", request);

        await _translationRepository.UpdateTranslationAsync(request.UpdatedBy, request.TableName, request.RecordId, request.Culture, request.Value, cancellationToken);

        return Unit.Value;
    }
}
