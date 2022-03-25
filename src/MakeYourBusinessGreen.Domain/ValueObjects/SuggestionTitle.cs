namespace MakeYourBusinessGreen.Domain.ValueObjects;
public record SuggestionTitle
{
    public string Value { get; }

    public SuggestionTitle(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EmptySuggestionTitleException();
        }

        if (value.Length > 50)
        {
            throw new TooLongSuggestionTitleException(50);
        }

        Value = value;
    }

    public static implicit operator string(SuggestionTitle suggestionTitle)
        => suggestionTitle.Value;

    public static implicit operator SuggestionTitle(string suggestionTitle)
        => new(suggestionTitle);
}
