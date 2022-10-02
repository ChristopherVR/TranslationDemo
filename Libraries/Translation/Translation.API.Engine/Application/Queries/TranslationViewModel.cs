namespace Translation.API.Engine.Application.Queries;

public sealed record TranslationPreview(
    string Culture,
    string? Value,
    long RecordId,
    string TableName,
    string FieldName);

public sealed record TablePreview(string TableName, IList<RecordPreview> Records);

public sealed record FieldPreview(string? Value, string Culture);

public sealed record RecordPreview(long RecordId, IList<(string FieldName, IList<FieldPreview> Data)> Values);
