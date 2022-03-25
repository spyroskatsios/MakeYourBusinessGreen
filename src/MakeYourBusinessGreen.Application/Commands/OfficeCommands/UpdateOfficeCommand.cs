namespace MakeYourBusinessGreen.Application.Commands.OfficeCommands;
public record UpdateOfficeCommand(Guid Id, string Name) : IRequest<bool>
{
}
