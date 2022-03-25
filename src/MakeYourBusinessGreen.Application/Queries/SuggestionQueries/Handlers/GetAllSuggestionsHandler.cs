namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries.Handlers;
public class GetAllSuggestionsHandler : IRequestHandler<GetAllSuggestionsQuery, PagedList<SuggestionResponse>>
{
    private readonly IReadDbContext _context;

    public GetAllSuggestionsHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<SuggestionResponse>> Handle(GetAllSuggestionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Suggestions
            .Include(x => x.StatusChangedEvents.OrderBy(x => x.DateTime))
            .Include(x => x.Office)
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderByDescending(x => x.Created);


        var suggestions = await query.ToListAsync(cancellationToken);
        var count = await _context.Suggestions.CountAsync(cancellationToken);

        var suggestionsResponse = suggestions.ToSuggestionsResponse();

        var pagedList = PagedList<SuggestionResponse>.ToPagedList(suggestionsResponse, request.PageNumber, request.PageSize, count);

        return pagedList;
    }
}
