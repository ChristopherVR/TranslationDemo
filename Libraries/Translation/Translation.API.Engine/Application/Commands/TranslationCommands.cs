using MediatR;

namespace Translation.API.Engine.Application.Commands;

public sealed record CreateTranslationCommand(
    string TableName,
    string FieldName,
    string Value,
    long RecordId,
    string Culture,
    string UpdatedBy) : IRequest<Unit>;

public sealed record UpdateTranslationCommand(
    string TableName,
    string FieldName,
    string Value,
    long RecordId,
    string Culture,
    string UpdatedBy) : IRequest<Unit>;

public sealed record DeleteTranslationCommand(
    string TableName,
    string FieldName,
    long RecordId,
    string Culture,
    string UpdatedBy) : IRequest<Unit>;
