namespace MakeYourBusinessGreen.Application.Commands.AuthCommands;
public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword) : IRequest<AuthResponse>
{

}
