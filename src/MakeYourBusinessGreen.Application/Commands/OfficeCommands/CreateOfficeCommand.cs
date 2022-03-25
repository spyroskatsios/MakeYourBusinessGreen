namespace MakeYourBusinessGreen.Application.Commands.OfficeCommands;
public record CreateOfficeCommand(string Name) : IRequest<Guid>
{
}


