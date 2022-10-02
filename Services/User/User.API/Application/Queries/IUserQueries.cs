namespace User.API.Application.Queries;

public interface IUserQueries
{
    Task<UserPreview?> GetAsync(int id);
    Task<List<UserPreview>> ListUsersAsync();
}
