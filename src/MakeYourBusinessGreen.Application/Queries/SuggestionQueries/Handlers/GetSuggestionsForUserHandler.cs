namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries.Handlers;
public class GetSuggestionsForUserHandler : IRequestHandler<GetSuggestionsForUserQuery, PagedList<SuggestionResponse>>
{
    private readonly IReadDbContext _context;

    public GetSuggestionsForUserHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<SuggestionResponse>> Handle(GetSuggestionsForUserQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Suggestions
            .AsNoTracking()
            .Include(x => x.Office)
            .Include(x => x.StatusChangedEvents.OrderBy(x => x.DateTime))
            .Where(x => x.UserId == request.UserId);

        var suggestions = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderByDescending(x => x.Created)
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        var suggestionsResponse = suggestions.ToSuggestionsResponse();

        var pagedList = PagedList<SuggestionResponse>.ToPagedList(suggestionsResponse, request.PageNumber, request.PageSize, count);

        return pagedList;
    }
}
