using Microsoft.Data.SqlClient;

namespace User.API.Application.Queries;

public class UserQueries : IUserQueries
{
    private readonly string _connectionString;

    public UserQueries(string connectionString)
    {
        _connectionString = !string.IsNullOrWhiteSpace(connectionString)
            ? connectionString
            : throw new ArgumentNullException(nameof(connectionString));
    }

    #region Users

    public async Task<UserPreview?> GetAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var sql = @"
                SELECT
                    [M].[Id]                                 [Id],
                    [M].[Culture]                            [Culture],
                    [M].[UserName]                           [UserName],
                    [M].[SiteId]                             [SiteId]
                FROM Config.Users M WITH(NOLOCK)
                WHERE [M].Id = @id";

        UserPreview? user = await connection.QueryFirstOrDefaultAsync<UserPreview>(
            sql,
            param: new
            {
                id,
            });

        return user;
    }
 
    public async Task<List<UserPreview>> ListUsersAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var sql = @"
                SELECT
                    [M].[Id]                                 [Id],
                    [M].[Culture]                            [Culture],
                    [M].[UserName]                           [UserName],
                    [M].[SiteId]                             [SiteId]
                FROM Config.Users M WITH(NOLOCK)";

        IEnumerable<UserPreview> users = await connection.QueryAsync<UserPreview>(
            sql);

        return users.AsList();
    }

    #endregion Users

}
