namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries.Handlers;
public class SearchSuggestionByTitleHandler : IRequestHandler<SearchSuggestionByTitleQuery, PagedList<SuggestionResponse>>
{
    private readonly IReadDbContext _context;

    public SearchSuggestionByTitleHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<SuggestionResponse>> Handle(SearchSuggestionByTitleQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Suggestions
            .AsNoTracking()
            .Include(x => x.Office)
            .Include(x => x.StatusChangedEvents.OrderBy(x => x.DateTime))
            .Where(x => x.Title.Contains(request.Title, StringComparison.InvariantCultureIgnoreCase));

        var suggestions = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(x => x.Title)
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        var sugestionsResponse = suggestions.ToSuggestionsResponse();

        var pagedList = PagedList<SuggestionResponse>.ToPagedList(sugestionsResponse, request.PageNumber, request.PageSize, count);

        return pagedList;
    }
}
