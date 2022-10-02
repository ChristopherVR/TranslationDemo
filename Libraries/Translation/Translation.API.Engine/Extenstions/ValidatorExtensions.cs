using FluentValidation;
using FluentValidation.Validators;

namespace Translation.API.Engine.Extenstions;

internal static class ValidatorExtensions
{
#pragma warning disable IDE1006 // Naming Styles
    internal static IRuleBuilderOptions<T, TProperty> NotDefault<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
#pragma warning restore IDE1006 // Naming Styles
    {
        return ruleBuilder.SetValidator(new NotDefaultValidator<T, TProperty>());
    }

}

#pragma warning disable IDE1006 // Naming Styles
public sealed class NotDefaultValidator<T, TProperty> : PropertyValidator<T, TProperty>, INotDefaultValidator
#pragma warning restore IDE1006 // Naming Styles
{
    public override string Name => "NotDefaultValidator";

    public override bool IsValid(ValidationContext<T> context, TProperty value) => !Equals(value, default(TProperty));

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return Localized(errorCode, Name);
    }
}

public interface INotDefaultValidator : IPropertyValidator
{
}

