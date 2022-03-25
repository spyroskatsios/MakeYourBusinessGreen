namespace MakeYourBusinessGreen.Application.Queries.OfficeQueries;
public record GetOfficeByIdQuery(Guid Id) : IRequest<OfficeResponse>
{
}
