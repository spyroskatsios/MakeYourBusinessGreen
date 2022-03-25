namespace MakeYourBusinessGreen.Application.Interfaces;
public interface IAuthService
{
    Task<AuthResult> AddUserToRoleAsync(AddUserToRoleCommand request);
    Task<AuthResult> ChangePasswordAsync(ChangePasswordCommand request, string userId);
    Task<AuthResult> ForgotPasswordAsync(ForgotPasswordCommand request);
    Task<AuthResult> RefreshTokenAsync(RefreshTokenCommand request);
    Task<AuthResult> ResetPasswordAsync(ResetPasswordCommand request);
    Task<AuthResult> SignInUserAsync(SignInCommand request);
    Task<AuthResult> SignUpUserAsync(SignUpCommand request);
}
