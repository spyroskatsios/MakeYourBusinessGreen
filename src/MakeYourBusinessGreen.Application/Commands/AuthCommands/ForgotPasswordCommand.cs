namespace MakeYourBusinessGreen.Application.Commands.AuthCommands;
public record ForgotPasswordCommand(string Email) : IRequest<AuthResponse>
{

}
