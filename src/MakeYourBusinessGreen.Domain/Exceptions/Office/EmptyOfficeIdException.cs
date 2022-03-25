namespace MakeYourBusinessGreen.Domain.Exceptions.Office;
public class EmptyOfficeIdException : OfficeException
{
    public EmptyOfficeIdException() : base("Office Id can not be empty.")
    {
    }
}
