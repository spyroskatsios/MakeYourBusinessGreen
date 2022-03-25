namespace MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
public abstract class SuggestionException : Exception
{
    protected SuggestionException(string message) : base(message)
    {
    }
}
