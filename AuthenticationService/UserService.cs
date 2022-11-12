using AuthenticationService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace AuthenticationService;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccesor;

    public UserService(IHttpContextAccessor httpContextAccesor)
        => _httpContextAccesor = httpContextAccesor ?? throw new ArgumentNullException(nameof(httpContextAccesor));

    public int? GetSiteId()
        => int.TryParse(_httpContextAccesor.HttpContext.User?.FindFirst("site_id")?.Value, out int res)
        ? res
        : default;

    private string? GetClaimCulture() => _httpContextAccesor.HttpContext?.User.FindFirst("locale")?.Value;

    public string GetUserCulture()
    {
        string userCulture = _httpContextAccesor.HttpContext?.Request.Query.TryGetValue("culture", out StringValues queryStringculture) ?? (false
            && !string.IsNullOrWhiteSpace(queryStringculture))
            ? queryStringculture.First()
            : string.Empty;

        if (string.IsNullOrWhiteSpace(userCulture))
        {
            string? localeClaim = GetClaimCulture();
            if (!string.IsNullOrWhiteSpace(localeClaim))
            {
                userCulture = localeClaim;
            }
            else
            {
                RequestHeaders? typedHeaders = _httpContextAccesor.HttpContext?.Request.GetTypedHeaders();
                IEnumerable<string>? headerValues = typedHeaders?.AcceptLanguage
                    .OrderByDescending(x => x.Quality ?? 1d)
                    .Select(x => x.Value.Value);

                userCulture = headerValues?.FirstOrDefault() ?? string.Empty;
            }
        }

        return userCulture!;
    }

    public string GetUsername() => _httpContextAccesor.HttpContext?.User.FindFirst("username")?.Value ?? "unknown";

    public int GetUserId() => int.Parse(_httpContextAccesor.HttpContext?.User.FindFirst("user_id")?.Value!);
}
