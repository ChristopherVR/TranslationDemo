namespace Translation.Infrastructure.Engine.Options;

internal class LocalizationOptions
{
    public const string FallbackCulture = Common.Culture.FallbackCulture;
    public string? DefaultCulture { get; set; }
}
