namespace MakeYourBusinessGreen.Application.Validators.Auth;
public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.ConfirmPassword).Equal(x => x.ConfirmPassword);
    }
}
