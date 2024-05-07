namespace Cower.Service.Exceptions;

public class EmailTakenException : BusinessLogicException
{
    public EmailTakenException(string? message = null) : base(message)
    {
    }
}