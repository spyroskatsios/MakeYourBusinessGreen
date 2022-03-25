namespace MakeYourBusinessGreen.Domain.Exceptions.Office;
public class TooLongOfficeNameException : OfficeException
{
    public TooLongOfficeNameException(int maxLenght) : base($"Office Name can not be more than {maxLenght} characters.")
    {
    }
}
