namespace MakeYourBusinessGreen.Application.Queries.OfficeQueries.Handlers;
public class GetAllOfficesHandler : IRequestHandler<GetAllOfficesQuery, PagedList<OfficeResponse>>
{
    private readonly IReadDbContext _context;

    public GetAllOfficesHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<OfficeResponse>> Handle(GetAllOfficesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Offices
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(x => x.Name);

        var offices = await query.ToListAsync(cancellationToken);
        var count = await _context.Offices.AsNoTracking().CountAsync(cancellationToken);

        var officesResponse = offices.ToOfficesResponse();

        var pagedList = PagedList<OfficeResponse>.ToPagedList(officesResponse, request.PageNumber, request.PageSize, count);

        return pagedList;
    }
}
