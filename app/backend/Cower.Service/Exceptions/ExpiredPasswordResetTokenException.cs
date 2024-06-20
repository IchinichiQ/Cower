namespace Cower.Service.Exceptions;

public class ExpiredPasswordResetTokenException : BusinessLogicException
{
    public ExpiredPasswordResetTokenException(string? message = null) : base(message)
    {
    }
}