using MakeYourBusinessGreen.Domain.Enums;

namespace MakeYourBusinessGreen.Application.Validators.Suggestion;
public class UpdateSuggestionValidator : AbstractValidator<UpdateSuggestionStatusCommand>
{
    public UpdateSuggestionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Status).IsEnumName(typeof(Status), caseSensitive: false);

        RuleFor(x => x.Details)
            .NotEmpty()
            .MaximumLength(500);
    }
}
