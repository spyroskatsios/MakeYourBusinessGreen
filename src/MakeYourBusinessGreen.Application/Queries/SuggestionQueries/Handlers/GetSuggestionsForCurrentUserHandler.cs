namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries.Handlers;
public class GetSuggestionsForCurrentUserHandler : IRequestHandler<GetSuggestionsForCurrentUserQuery, PagedList<SuggestionResponse>>
{
    private readonly IReadDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSuggestionsForCurrentUserHandler(IReadDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PagedList<SuggestionResponse>> Handle(GetSuggestionsForCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id;

        var query = _context.Suggestions
            .AsNoTracking()
            .Include(x => x.Office)
            .Include(x => x.StatusChangedEvents.OrderBy(x => x.DateTime))
            .Where(x => x.UserId == userId);

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
