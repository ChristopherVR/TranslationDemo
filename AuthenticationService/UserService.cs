using AuthenticationService.Interfaces;

namespace AuthenticationService;

public class UserService : IUserService
{
    public int? GetSiteId()
    {
        return null;
    }

    public string GetUserCulture()
    {
        return "de-DE";
    }

    public string GetUsername()
    {
        throw new NotImplementedException();
    }
}
