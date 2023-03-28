namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries.Handlers;
public class GetSuggestionByIdHandler : IRequestHandler<GetSuggestionByIdQuery, SuggestionResponse?>
{
    private readonly IReadDbContext _context;

    public GetSuggestionByIdHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<SuggestionResponse?> Handle(GetSuggestionByIdQuery request, CancellationToken cancellationToken)
    {
        var suggestion = await _context.Suggestions
            .Include(x => x.Office)
            .Include(x => x.StatusChangedEvents.OrderBy(x => x.DateTime))
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        var suggestionResponse = suggestion.ToSuggestionResponse();
        return suggestionResponse;
    }
}
