namespace MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
public class EmptySuggestionIdException : SuggestionException
{
    public EmptySuggestionIdException() : base("Suggestion Id can not be empty")
    {
    }
}
