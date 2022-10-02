using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Translation.UnitTests")]
[assembly: InternalsVisibleTo("Translation.Infrastructure.Engine")]
namespace Translation.Domain.Engine.SeedWork;

internal class LocalizedField
{
    public string? Value { get; set; }
    public required string Culture { get; set; }
    public required string Field { get; set; }
}
