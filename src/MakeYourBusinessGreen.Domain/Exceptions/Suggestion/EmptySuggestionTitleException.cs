namespace MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
public class EmptySuggestionTitleException : SuggestionException
{
    public EmptySuggestionTitleException() : base("Suggestion Title can not be empty")
    {
    }
}
