namespace MakeYourBusinessGreen.Domain.Exceptions;
public class StatusChangedEventException : Exception
{
    public StatusChangedEventException(string message) : base(message)
    {
    }
}
