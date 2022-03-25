namespace MakeYourBusinessGreen.Application.Commands.SuggestionCommands;
public record DeleteSuggestionCommand(Guid Id) : IRequest<bool>
{

}
