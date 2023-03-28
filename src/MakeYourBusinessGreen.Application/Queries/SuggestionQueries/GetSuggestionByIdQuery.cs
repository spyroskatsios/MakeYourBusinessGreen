namespace MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
public record GetSuggestionByIdQuery(Guid Id) : IRequest<SuggestionResponse?>
{

}
