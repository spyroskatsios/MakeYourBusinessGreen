namespace MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
public class TooLongSuggestionBodyException : SuggestionException
{
    public TooLongSuggestionBodyException(int maximumLenght) : base($"Suggestion Body can not be more than {maximumLenght} characters.")
    {
    }
}
