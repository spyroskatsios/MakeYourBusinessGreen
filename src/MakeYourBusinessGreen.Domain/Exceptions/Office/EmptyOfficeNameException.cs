namespace MakeYourBusinessGreen.Domain.Exceptions.Office;
public class EmptyOfficeNameException : OfficeException
{
    public EmptyOfficeNameException() : base("Office Name can not be empty")
    {
    }
}
