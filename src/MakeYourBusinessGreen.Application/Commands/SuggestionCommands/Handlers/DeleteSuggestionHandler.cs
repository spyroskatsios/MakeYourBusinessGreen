namespace MakeYourBusinessGreen.Application.Commands.SuggestionCommands.Handlers;
public class DeleteSuggestionHandler : IRequestHandler<DeleteSuggestionCommand, bool>
{
    private readonly IRepositoryManager _repository;

    public DeleteSuggestionHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }


    public async Task<bool> Handle(DeleteSuggestionCommand request, CancellationToken cancellationToken)
    {
        var suggestion = await _repository.Suggestion.GetAsync(request.Id);

        if (suggestion is null)
        {
            return false;
        }

        await _repository.Suggestion.DeleteAsync(suggestion);

        return true;
    }
}
