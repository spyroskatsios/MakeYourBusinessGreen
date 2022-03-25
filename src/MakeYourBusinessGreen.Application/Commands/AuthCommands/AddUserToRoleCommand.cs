namespace MakeYourBusinessGreen.Application.Commands.AuthCommands;
public record AddUserToRoleCommand(string UserId, string RoleName) : IRequest<AuthResponse>
{

}
