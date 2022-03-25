namespace MakeYourBusinessGreen.Application.Commands.SuggestionCommands;
public record CreateSuggestionCommand(string Title, string Body, Guid OfficeId) : IRequest<Guid>
{

}
