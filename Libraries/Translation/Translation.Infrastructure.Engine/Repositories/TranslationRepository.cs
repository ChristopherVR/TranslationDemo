using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasterDataEngine.Repositories;
internal class TranslationRepository<T> : ITranslationRepository where T : DbContext
{
    private readonly ILogger<TranslationRepository<T>> _logger;
    private readonly T _context;
    public TranslationRepository(T context, ILogger<TranslationRepository<T>> logger)
        => (_context, _logger) = (context ?? throw new ArgumentNullException(nameof(context)), logger ?? throw new ArgumentNullException(nameof(logger)));


    public Task CreateTranslationAsync(string username, string tableName, long recordId, string culture, string value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTranslationAsync(string username, string tableName, long recordId, string culture, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTranslationAsync(string username, string tableName, long recordId, string culture, string value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
