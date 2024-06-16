namespace Cower.Service.Exceptions;

public class PasswordResetTokenDoesntExistException : BusinessLogicException
{
    public PasswordResetTokenDoesntExistException(string? message = null) : base(message)
    {
    }
}