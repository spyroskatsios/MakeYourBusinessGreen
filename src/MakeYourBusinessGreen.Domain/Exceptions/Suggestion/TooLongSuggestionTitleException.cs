namespace MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
public class TooLongSuggestionTitleException : SuggestionException
{
    public TooLongSuggestionTitleException(int maximumLenght) : base($"Suggestion Title can not be more than {maximumLenght} characters.")
    {
    }

}
