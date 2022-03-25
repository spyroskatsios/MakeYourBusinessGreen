namespace MakeYourBusinessGreen.Domain.ValueObjects;
public record OfficeId
{
    public Guid Value { get; set; }

    public OfficeId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new EmptyOfficeIdException();
        }

        Value = value;
    }

    public static implicit operator Guid(OfficeId officeId)
        => officeId.Value;

    public static implicit operator OfficeId(Guid officeId)
        => new(officeId);
}
