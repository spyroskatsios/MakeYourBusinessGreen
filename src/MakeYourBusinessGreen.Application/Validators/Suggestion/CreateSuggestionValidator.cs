namespace MakeYourBusinessGreen.Application.Validators.Suggestion;
public class CreateSuggestionValidator : AbstractValidator<CreateSuggestionCommand>
{
    public CreateSuggestionValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .NotEmpty();


        RuleFor(x => x.Body)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(x => x.OfficeId)
            .NotEmpty();
    }
}
