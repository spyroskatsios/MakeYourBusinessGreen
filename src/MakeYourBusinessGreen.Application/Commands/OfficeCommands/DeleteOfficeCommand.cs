namespace MakeYourBusinessGreen.Application.Commands.OfficeCommands;
public record DeleteOfficeCommand(Guid Id) : IRequest<bool>
{

}
