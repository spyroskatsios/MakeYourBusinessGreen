namespace MakeYourBusinessGreen.Application.Commands.AuthCommands;
public record SignUpCommand(string Email, string UserName, string FirstName, string LastName) : IRequest<AuthResponse>
{

}
