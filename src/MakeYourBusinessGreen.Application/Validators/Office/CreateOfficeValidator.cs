namespace MakeYourBusinessGreen.Application.Validators.Office;
public class CreateOfficeValidator : AbstractValidator<CreateOfficeCommand>
{
    public CreateOfficeValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();


    }
}
