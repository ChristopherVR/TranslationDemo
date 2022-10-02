using MasterDataEngine.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Translation.API.Engine.Application.Commands;
internal sealed class DeleteTranslationCommandHandler : IRequestHandler<DeleteTranslationCommand, Unit>
{
    private readonly ILogger<DeleteTranslationCommandHandler> _logger;
    private readonly ITranslationRepository _translationRepository;

    public DeleteTranslationCommandHandler(ILogger<DeleteTranslationCommandHandler> logger, ITranslationRepository translationRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
    }

    public async Task<Unit> Handle(DeleteTranslationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting translation {request}", request);

        await _translationRepository.DeleteTranslationAsync(request.UpdatedBy, request.TableName, request.RecordId, request.Culture, cancellationToken);

        return Unit.Value;
    }
}
