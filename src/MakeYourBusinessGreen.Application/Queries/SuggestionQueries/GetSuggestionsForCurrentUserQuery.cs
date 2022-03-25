namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
public record GetSuggestionsForCurrentUserQuery() : RequestParameters, IRequest<PagedList<SuggestionResponse>>
{

}
