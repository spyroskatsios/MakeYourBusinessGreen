namespace MakeYourBusinessGreen.Domain.ValueObjects;
public record SuggestionBody
{
    public string Value { get; }

    public SuggestionBody(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EmptySuggestionBodyException();
        }

        if (value.Length > 200)
        {
            throw new TooLongSuggestionBodyException(200);
        }

        Value = value;
    }

    public static implicit operator string(SuggestionBody suggestionBody)
        => suggestionBody.Value;

    public static implicit operator SuggestionBody(string suggestionBody)
        => new(suggestionBody);
}
