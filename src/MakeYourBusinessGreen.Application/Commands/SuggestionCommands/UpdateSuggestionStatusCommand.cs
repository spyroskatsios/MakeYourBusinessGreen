namespace MakeYourBusinessGreen.Application.Commands.SuggestionCommands;
public record UpdateSuggestionStatusCommand(Guid Id, string Status, string Details) : IRequest<bool>
{

}
