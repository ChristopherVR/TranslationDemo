namespace Translation.Domain.Engine.SeedWork;

public sealed class LocalizedString
{
    public LocalizedString(string? value, string culture)
    {
        Value = value;
        Culture = culture;
    }

    public string? Value { get; private set; }
    public string Culture { get; private set; }
}
