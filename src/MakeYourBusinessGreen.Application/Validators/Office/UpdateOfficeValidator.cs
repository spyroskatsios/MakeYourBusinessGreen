namespace MakeYourBusinessGreen.Application.Validators.Office;
public class UpdateOfficeValidator : AbstractValidator<UpdateOfficeCommand>
{
    public UpdateOfficeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
           .MaximumLength(50)
           .NotEmpty();

    }
}
