namespace MakeYourBusinessGreen.Application.Commands.UserCommands;
public record UpdateUserCommand(string Id, string FirstName, string LastName) : IRequest<bool>
{

}
