using MakeYourBusinessGreen.Domain.Enums;

namespace MakeYourBusinessGreen.Application.Commands.SuggestionCommands.Handlers;
public class UpdateSuggestionStatusHandler : IRequestHandler<UpdateSuggestionStatusCommand, bool>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateSuggestionStatusHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(UpdateSuggestionStatusCommand request, CancellationToken cancellationToken)
    {
        var suggestion = await _repository.Suggestion.GetAsync(request.Id);


        if (suggestion is null)
        {
            return false;
        }

        suggestion.ChangeStatus((Status)Enum.Parse(typeof(Status), request.Status), request.Details, _currentUserService.Id);

        await _repository.Suggestion.UpdateAsync(suggestion);

        return true;
    }
}
