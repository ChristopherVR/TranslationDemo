using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Translation.API.Engine.Application.Queries;
#pragma warning disable IDE1006 // Naming Styles
public sealed class TranslationQueries<TDbContext> : ITranslationQueries where TDbContext : DbContext
#pragma warning restore IDE1006 // Naming Styles
{
    private readonly string _connectionString;
    private readonly DbContext _dbContext;

    public TranslationQueries(string connectionString, TDbContext dbContext)
    {
        _connectionString = !string.IsNullOrWhiteSpace(connectionString)
            ? connectionString
            : throw new ArgumentNullException(nameof(connectionString));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        // var oldMap = SqlMapper.GetTypeMap(typeof(SomeType));
        // var map = new CustomTypeMap(typeof(SomeType), oldMap);
        // map.Map("IFoo", "Foo");
        // map.Map("SBar", "Bar");
        // SqlMapper.SetTypeMap(map.Type, map);
    }

    #region Translations

    public async Task<TranslationPreview?> GetAsync(long id, string tableName, string fieldName, string? culture = default, CancellationToken cancellationToken = default) // User culture is used if none provided
    {
        ArgumentNullException.ThrowIfNull(tableName);
        ArgumentNullException.ThrowIfNull(fieldName);

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var entity = _dbContext.Model.FindEntityType(tableName);

        if (entity is null)
        {
            throw new Exception(); // TODO: Specific exception with error code.
        }

        string schemaQualifiedName = entity.GetSchemaQualifiedTableName()!;

        string primaryIdentifier = entity
            .GetProperties()
            .First(y => y.IsPrimaryKey())
            .GetColumnName();

        string field = entity.FindProperty(fieldName)?.GetColumnName() ?? throw new Exception(); // TODO: SPecific exception.

        var sql = @$"
                SELECT
                    [M].[{field}] [Value]
                FROM {schemaQualifiedName} M WITH(NOLOCK)
                WHERE [M].[{primaryIdentifier}] = @id";

        TranslationPreview? translation = await connection.QueryFirstOrDefaultAsync<TranslationPreview>(
            sql,
            param: new
            {
                id,
            });

        return translation;
    }

    public class LocalizedField
    {
        public string? Value { get; init; }
        public string Culture { get; init; } = null!;
    }

    public class LocalizedString
    {
        public string? Value { get; init; }
        private IList<LocalizedField> _fields = new List<LocalizedField>();
        public static implicit operator string?(LocalizedString? s) => s?.Value;

        public static implicit operator LocalizedString?(string? s) => new()
        {
            Value = s,
            _fields = new List<LocalizedField>(),
        };

        public override string? ToString() => _fields.FirstOrDefault(y => y.Culture == null)?.Value;

        public string? ToString(string? culture) => _fields.FirstOrDefault(y => y.Culture == culture)?.Value;
    }


    public class TranslationTypeHandler : SqlMapper.TypeHandler<LocalizedString>
    {
        public override LocalizedString Parse(object value)
        {
            try
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<List<LocalizedField>>(value.ToString()!);

                return new LocalizedString()
                {
                    // Value = data,
                };
            }
            catch
            {
                return value.ToString()!;
            }
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, LocalizedString value) => parameter.Value = value;
    }


    public async Task<List<TablePreview>> ListAllTranslationsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var sql = @$"";

        IEnumerable<TablePreview> translations = await connection.QueryAsync<TablePreview>(
            sql);

        return translations.AsList();
    }

    public async Task<List<TranslationPreview>> ListTranslationsAsync(string tableName, string fieldName, CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var entity = _dbContext.Model.FindEntityType(tableName);

        if (entity is null)
        {
            throw new Exception(); // TODO: Specific exception with error code.
        }

        string schemaQualifiedName = entity.GetSchemaQualifiedTableName()!;

        string primaryIdentifier = entity
            .GetProperties()
            .First(y => y.IsPrimaryKey())
            .GetColumnName();

        string field = entity.FindProperty(fieldName)?.GetColumnName() ?? throw new Exception(); // TODO: SPecific exception.

        var sql = @$"
                SELECT
                    [M].[{field}] [Value]
                FROM {schemaQualifiedName} M WITH(NOLOCK)
                WHERE [M].[{primaryIdentifier}] = @id";

        IEnumerable<TranslationPreview> translations = await connection.QueryAsync<TranslationPreview>(
            sql);

        return translations.AsList();
    }

    #endregion Translations

}
