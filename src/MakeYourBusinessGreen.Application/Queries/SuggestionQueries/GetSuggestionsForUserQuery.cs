namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
public record GetSuggestionsForUserQuery(string UserId) : RequestParameters, IRequest<PagedList<SuggestionResponse>>
{

}
