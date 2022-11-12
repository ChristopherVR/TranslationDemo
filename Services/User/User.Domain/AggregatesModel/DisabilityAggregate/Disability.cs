using Domain.Extensions.SeedWork;
using Translation.Domain.Engine.SeedWork;

namespace User.Domain.AggregatesModel.UserAggregate;
public class Disability : LocalizedEntity, IAggregateRoot
{
    public string UpdatedUser { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    public List<LocalizedString> Name { get; private set; }
    public List<LocalizedString> Description { get; private set; }

    /// <summary>
    ///  Default EF constructor
    /// </summary>
    /// 
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Disability()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }

    private Disability(
        string createdBy,
        List<LocalizedString> name,
        List<LocalizedString> description)
    {
        UpdatedDate = DateTime.MinValue;
        UpdatedUser = createdBy;
        Name = name;
        Description = description;
    }

    public static List<Disability> InitialSeedData() => new()
    {
        new Disability("Micration", new()
        {
            new("en-US","Deaf")
           ,
            new("en-GB", "Not Dead"),
        },
            new()
            {
                new("en-US", "Deaf's description"),
                new("en-GB", "Deaf GB's description")
            })
        {
            Id = 1
        },
        // new("Migration", MapRawData("Deaf", "en-US"), MapRawData("This person is deaf.", "en-US")),
    };
}
