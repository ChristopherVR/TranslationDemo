using FluentValidation;
using Microsoft.Extensions.Logging;
using Translation.API.Engine.Application.Commands;
using Translation.API.Engine.Extenstions;

namespace Translation.API.Engine.Application.Validations;
public sealed class CreateTranslationCommandValidator : AbstractValidator<CreateTranslationCommand>
{
    public CreateTranslationCommandValidator(ILogger<CreateTranslationCommandValidator> logger)
    {
        logger.LogInformation("UpdateTranslationCommandValidator is executing...");
        // TODO: Translation error codes?
        RuleFor(command => command.Value).NotEmpty().WithMessage("Value cannot be empty");
        RuleFor(command => command.Culture).NotEmpty();
        RuleFor(command => command.UpdatedBy).NotEmpty();
        RuleFor(command => command.Culture).MaximumLength(5).MinimumLength(2);
        RuleFor(command => command.UpdatedBy).NotEmpty();
        RuleFor(command => command.RecordId).NotDefault();
    }
}
