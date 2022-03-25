namespace MakeYourBusinessGreen.Domain.ValueObjects;
public record OfficeName
{
    public string Value { get; }

    public OfficeName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EmptyOfficeNameException();
        }

        if (value.Length > 50)
        {
            throw new TooLongOfficeNameException(50);
        }

        Value = value;
    }

    public static implicit operator string(OfficeName officeName)
        => officeName.Value;

    public static implicit operator OfficeName(string officeName)
        => new(officeName);
}
