namespace MakeYourBusinessGreen.Application.Validators.Auth;
public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.ConfirmPassword).Equal(x => x.ConfirmPassword);
    }
}
