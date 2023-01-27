namespace AuthenticationService.Interfaces;

public interface IUserService
{
    string GetUserCulture();
    string GetUsername();
    int? GetSiteId();
    int GetUserId();
}
