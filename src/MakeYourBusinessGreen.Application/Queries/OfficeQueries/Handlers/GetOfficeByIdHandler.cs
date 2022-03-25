namespace MakeYourBusinessGreen.Application.Queries.OfficeQueries.Handlers;
public class GetOfficeByIdHandler : IRequestHandler<GetOfficeByIdQuery, OfficeResponse>
{
    private readonly IReadDbContext _context;

    public GetOfficeByIdHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<OfficeResponse> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
    {
        var office = await _context.Offices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        var officeResponse = office.ToOfficeResponse();
        return officeResponse;
    }
}
