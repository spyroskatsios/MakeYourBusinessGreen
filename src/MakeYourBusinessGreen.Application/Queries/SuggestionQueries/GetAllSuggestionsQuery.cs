namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
public record GetAllSuggestionsQuery : RequestParameters, IRequest<PagedList<SuggestionResponse>>
{

}
