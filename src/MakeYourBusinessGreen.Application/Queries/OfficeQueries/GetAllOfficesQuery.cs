namespace MakeYourBusinessGreen.Application.Queries.OfficeQueries;
public record GetAllOfficesQuery : RequestParameters, IRequest<PagedList<OfficeResponse>>
{

}
