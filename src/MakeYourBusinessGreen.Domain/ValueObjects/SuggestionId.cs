namespace MakeYourBusinessGreen.Domain.ValueObjects;
public record SuggestionId
{
    public Guid Value { get; set; }

    public SuggestionId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new EmptySuggestionIdException();
        }

        Value = value;
    }

    public static implicit operator Guid(SuggestionId suggestionId)
        => suggestionId.Value;

    public static implicit operator SuggestionId(Guid suggestionId)
        => new(suggestionId);
}
