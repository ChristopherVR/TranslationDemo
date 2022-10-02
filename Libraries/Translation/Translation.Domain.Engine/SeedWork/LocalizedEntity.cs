using Domain.Extensions.SeedWork;
using System.Globalization;
using System.Runtime.CompilerServices;
using Translation.Domain.Engine.Events;
using Translation.Domain.Engine.Exceptions;

[assembly: InternalsVisibleTo("Translation.UnitTests")]
[assembly: InternalsVisibleTo("Translation.Infrastructure.Engine")]
namespace Translation.Domain.Engine.SeedWork;

public abstract class LocalizedEntity : Entity
{
    internal string? UserCulture { get; set; }
    /// <summary>
    /// This is the site culture that is set when the entities are loaded.
    /// </summary>
    internal string? SiteCulture { get; set; }
    /// <summary>
    /// This is a worst-case scenario fallback language used to know what value to retrieve for the entity.
    /// This value can be set when the entitiesa are loaded, otherwise en-US is the default.
    /// </summary>
    internal string FallbackCulture { get; set; } = Common.Culture.FallbackCulture;

    /// <summary>
    /// Contains a list of all the shadow properties' values
    /// </summary>
    internal readonly IList<LocalizedField> _fields = new List<LocalizedField>();

    public LocalizedEntity()
    {
        AddDomainEvent(new TranslationCreatedDomainEvent(this));
    }

    public string? GetValue(Func<LocalizedEntity, IComparable> property, string? culture = default, bool useFallback = true)
        => GetValue(property(this)?.ToString() ?? throw new ArgumentNullException(nameof(property)), culture, useFallback);

    public string? GetValue(string field, string? culture = default, bool useFallback = true)
    {
        if (culture is not null && !CultureExists(culture))
        {
            throw new LocalizedEntityException("Invalid culture passed to entity", Constants.ErrorCodes.InvalidCultureOnEntityCode);
        }

        culture ??= GetUserCulture();

        LocalizedField? value = _fields?
        .FirstOrDefault(y => y.Culture.Equals(culture, StringComparison.InvariantCultureIgnoreCase) && y.Field == field);

        value ??= useFallback
            ? _fields?.FirstOrDefault(u => u.Culture.Equals(SiteCulture ?? FallbackCulture, StringComparison.InvariantCultureIgnoreCase) && u.Field == field)
            : value;

        return value?.Value;
    }

    /// <summary>
    /// Determines if the field value is unique for all cultures. This can be used in cases where even though the culture differs
    /// the value still needs to remain unique.
    /// </summary>
    /// <returns></returns>
    public bool ValidUniqueValue(Func<LocalizedEntity, IComparable> property, string? value)
        => ValidUniqueValue(property(this)?.ToString() ?? throw new ArgumentNullException(nameof(property)), value);

    /// <summary>
    /// Determines if the field value is unique for all cultures. This can be used in cases where even though the culture differs
    /// the value still needs to remain unique.
    /// </summary>
    /// <returns></returns>
    public bool ValidUniqueValue(string property, string? value, bool ignoreNulls = false)
    {
        var values = _fields?.Where(y => y.Field == property && (!ignoreNulls || y.Value is not null))?.Select(y => y.Value);

        if (!values?.Any() ?? false)
        {
            throw new LocalizedEntityException("No localized field on entity.", Constants.ErrorCodes.NoLocalizedFieldsOnEntityCode);
        }

        return values?.Any(y => y == value) ?? false;
    }

    internal static bool CultureExists(string name)
        => CultureInfo.GetCultures(CultureTypes.AllCultures).Any(y => y.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

    /// <summary>
    /// The user culture will be set by the Infrastructure project when the entities are loaded. In the event they are not loaded, the <see cref="Thread.CurrentThread"/> will be used to determine the user culture.
    /// </summary>
    /// <returns>User culture.</returns>
    internal string GetUserCulture()
    {
        if (UserCulture is not null)
        {
            return UserCulture;
        }

        return Thread.CurrentThread.CurrentUICulture.Name;
    }
}
