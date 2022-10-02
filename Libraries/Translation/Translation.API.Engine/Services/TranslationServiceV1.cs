using AuthenticationService.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Translation.API.Engine.Application.Commands;
using Translation.API.Engine.Application.Queries;
using GrpcTranslation.V1;


namespace Translation.API.Engine.Services;
[Authorize]
public sealed class TranslationServiceV1 : GrpcTranslation.V1.Translations.TranslationsBase
{
    private readonly ILogger<TranslationServiceV1> _logger;
    private readonly IMediator _mediator;
    private readonly ITranslationQueries _translationsQueries;
    private readonly IUserService _userService;

    public TranslationServiceV1(
        ILogger<TranslationServiceV1> logger,
        IMediator mediator,
        ITranslationQueries movieQueries,
        IUserService userService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _translationsQueries = movieQueries ?? throw new ArgumentNullException(nameof(movieQueries));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    #region Translations

    public override async Task<Empty> DeleteTranslation(DeleteTranslationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Deleting translation with Id - {id}", request.RecordId);

        var updatedBy = _userService.GetUsername();
        _ = await _mediator.Send(new DeleteTranslationCommand(request.TableName, request.FieldName, request.RecordId, request.Culture, updatedBy), context.CancellationToken);

        return new();
    }

    public override async Task<Empty> CreateTranslation(CreateTranslationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Creating translation with Value - {value}", request.Value);
        var updatedBy = _userService.GetUsername();

        var command = new CreateTranslationCommand(
            request.TableName,
            request.FieldName,
            request.Value,
            request.RecordId,
            request.Culture,
            updatedBy);

        _ = await _mediator.Send(command, context.CancellationToken);

        return new();
    }

    public override async Task<Empty> UpdateTranslation(UpdateTranslationRequest request, ServerCallContext context)
    {
        var updatedBy = _userService.GetUsername();

        var command = new UpdateTranslationCommand(
            request.TableName,
            request.FieldName,
            request.Value,
            request.RecordId,
            request.Culture,
            updatedBy);

        _ = await _mediator.Send(command, context.CancellationToken);

        return new();
    }

    public override async Task<ListTranslationsResponse> ListTranslations(ListTranslationsRequest request, ServerCallContext context)
    {
        List<TranslationPreview> translations = await _translationsQueries
           .ListTranslationsAsync(request.TableName, request.FieldName, context.CancellationToken);

        return new()
        {
            Translations = 
            {
                translations.Select(y => new GrpcTranslation.V1.ExtendedTranslation
                {
                    Value = y.Value,
                    TableName = y.TableName,
                    FieldName = y.FieldName,
                    Culture = y.Culture,
                }),
            }
        };
    }

    public override async Task<GrpcTranslation.V1.Translation> GetTranslation(GetTranslationRequest request, ServerCallContext context)
    {
        TranslationPreview? translation = await _translationsQueries.GetAsync(
            request.RecordId,
            request.TableName, 
            request.FieldName, 
            request.Culture, 
            context.CancellationToken);

        if (translation is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Translation not found"));
        }

        return new()
        {
            Value = translation.Value,
            Culture = translation.Culture,
        };
    }

    #endregion Translations
}
