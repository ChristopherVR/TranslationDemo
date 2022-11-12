using Domain.Extensions.SeedWork;

namespace User.Domain.AggregatesModel.UserAggregate;
public class User : Entity, IAggregateRoot
{
    public string UpdatedUser { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    public string? Culture { get; private set; }
    public string Username { get; private set; }
    public int SiteId { get; private set; }

    private readonly List<UserDisability> _userDisabilities = new();
    public IReadOnlyCollection<UserDisability> UserDisabilities => _userDisabilities;

    /// <summary>
    ///  Default EF constructor
    /// </summary>
    /// 
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }

    private User(
        string username,
        string? culture,
        int siteId,
        string createdBy)
    {
        UpdatedDate = DateTime.MinValue;
        UpdatedUser = createdBy;
        Username = username;
        Culture = culture;
        SiteId = siteId;
    }

    public static List<User> InitialSeedData() => new()
    {
        new("ChristopherVR", "en-US", 1, "Migrations") { Id = 1 },
    };
}
