namespace Translation.API.Engine.Application.Queries;

public interface ITranslationQueries
{
    Task<TranslationPreview?> GetAsync(long id, string tableName, string fieldName, string? culture = default, CancellationToken cancellationToken = default);
    Task<List<TablePreview>> ListAllTranslationsAsync(CancellationToken cancellationToken = default);
    Task<List<TranslationPreview>> ListTranslationsAsync(string tableName, string fieldName, CancellationToken cancellationToken = default);
}
