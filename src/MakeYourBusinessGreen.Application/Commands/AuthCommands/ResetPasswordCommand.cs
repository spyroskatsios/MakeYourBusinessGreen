namespace MakeYourBusinessGreen.Application.Commands.AuthCommands;
public record ResetPasswordCommand(string Token, string Email, string Password, string ConfirmPassword) : IRequest<AuthResponse>
{

}
