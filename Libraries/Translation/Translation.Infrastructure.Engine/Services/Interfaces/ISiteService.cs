namespace Translation.Infrastructure.Engine.Services;

internal interface ISiteService
{
    public Task<string?> GetSiteCultureAsync();
}
