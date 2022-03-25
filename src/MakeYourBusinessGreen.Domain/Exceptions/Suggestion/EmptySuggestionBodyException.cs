namespace MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
public class EmptySuggestionBodyException : SuggestionException
{
    public EmptySuggestionBodyException() : base("Suggestion Body can not be empty")
    {
    }
}
