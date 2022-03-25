namespace MakeYourBusinessGreen.Application.Commands.AuthCommands;
public record SignInCommand(string Email, string Password) : IRequest<AuthResponse>
{

}
