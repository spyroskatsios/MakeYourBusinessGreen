using MakeYourBusinessGreen.Domain.Enums;

namespace MakeYourBusinessGreen.Application.Commands.SuggestionCommands.Handlers;
public class CreateSuggestionHandler : IRequestHandler<CreateSuggestionCommand, Guid>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public CreateSuggestionHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateSuggestionCommand request, CancellationToken cancellationToken)
    {
        var office = await _repository.Office.GetAsync(request.OfficeId);

        if (office is null)
        {
            return Guid.Empty;
        }

        var userId = _currentUserService.Id;

        if (string.IsNullOrWhiteSpace(userId) || userId == Guid.Empty.ToString())
        {
            return Guid.Empty;
        }

        var suggestion = new Suggestion(Guid.NewGuid(), request.Title, request.Body, office, Status.Pending, userId);

        await _repository.Suggestion.AddAsync(suggestion);

        return suggestion.Id;
    }
}
