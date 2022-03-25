namespace MakeYourBusinessGreen.Application.Queries.OfficeQueries.Handlers;
public class SearchOfficeByNameHandler : IRequestHandler<SearchOfficeByNameQuery, PagedList<OfficeResponse>>
{
    private readonly IReadDbContext _context;

    public SearchOfficeByNameHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<OfficeResponse>> Handle(SearchOfficeByNameQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Offices
            .AsNoTracking()
            .Where(x => x.Name.Contains(request.Name, StringComparison.InvariantCultureIgnoreCase));

        var offices = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        var officesResponse = offices.ToOfficesResponse();

        var pagedList = PagedList<OfficeResponse>.ToPagedList(officesResponse, request.PageNumber, request.PageSize, count);

        return pagedList;
    }
}
