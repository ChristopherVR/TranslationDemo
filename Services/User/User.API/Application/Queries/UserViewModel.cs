namespace User.API.Application.Queries;

public sealed record UserPreview(
    int Id,
    string? Culture,
    string Username,
    int SiteId);
