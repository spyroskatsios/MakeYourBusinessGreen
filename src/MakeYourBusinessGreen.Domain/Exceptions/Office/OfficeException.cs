namespace MakeYourBusinessGreen.Domain.Exceptions.Office;
public abstract class OfficeException : Exception
{
    protected OfficeException(string message) : base(message)
    {
    }
}
