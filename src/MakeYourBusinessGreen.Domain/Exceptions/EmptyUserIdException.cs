namespace MakeYourBusinessGreen.Domain.Exceptions;
public class EmptyUserIdException : Exception
{
    public EmptyUserIdException() : base("User Id can not be empty.")
    {
    }
}
