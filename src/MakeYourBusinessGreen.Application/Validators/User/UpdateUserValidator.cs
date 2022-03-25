namespace MakeYourBusinessGreen.Application.Validators.User;
public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.FirstName)
          .MaximumLength(50)
          .NotEmpty();

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .NotEmpty();
    }
}
