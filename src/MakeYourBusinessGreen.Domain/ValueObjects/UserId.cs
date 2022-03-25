namespace MakeYourBusinessGreen.Domain.ValueObjects;
public record UserId
{
    public string Value { get; }

    public UserId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyUserIdException();
        }

        if (Guid.Parse(value) == Guid.Empty)
        {
            throw new EmptyUserIdException();
        }

        Value = value;
    }

    public static implicit operator string(UserId userId)
        => userId.Value;

    public static implicit operator UserId(string userId)
        => new(userId);
}
