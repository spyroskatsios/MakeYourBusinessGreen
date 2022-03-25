namespace MakeYourBusinessGreen.Application.Queries.OfficeQueries;
public record SearchOfficeByNameQuery(string Name) : RequestParameters, IRequest<PagedList<OfficeResponse>>
{
}

