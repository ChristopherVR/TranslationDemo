namespace Translation.Infrastructure.Engine.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public class LocalizedAttribute : Attribute
{
    public required string PropertyName { get; set; }
}
