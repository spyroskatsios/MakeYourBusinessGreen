namespace MakeYourBusinessGreen.Domain.Exceptions.Suggestion;
public class NullOfficeException : SuggestionException
{
    public NullOfficeException() : base("Suggetsion's Office can not be null.")
    {
    }
}
