namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
public record SearchSuggestionByTitleQuery(string Title) : RequestParameters, IRequest<PagedList<SuggestionResponse>>
{

}
