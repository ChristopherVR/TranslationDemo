using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Translation.UnitTests")]
[assembly: InternalsVisibleTo("Translation.API.Engine")]
namespace Translation.Infrastructure.Repositories;
internal interface ITranslationRepository
{
    Task DeleteTranslationAsync(string username, string tableName, long recordId, string culture, CancellationToken cancellationToken = default);
    Task UpdateTranslationAsync(string username, string tableName, long recordId, string culture, string value, CancellationToken cancellationToken = default);
    Task CreateTranslationAsync(string username, string tableName, long recordId, string culture, string value, CancellationToken cancellationToken = default);
}
