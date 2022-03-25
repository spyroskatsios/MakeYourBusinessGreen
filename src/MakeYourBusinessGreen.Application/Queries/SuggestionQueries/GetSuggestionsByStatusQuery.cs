namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
public record GetSuggestionsByStatusQuery(string Status) : RequestParameters, IRequest<PagedList<SuggestionResponse>>
{

}
