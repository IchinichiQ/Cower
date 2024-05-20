namespace Cower.Service.Exceptions;

public class ForbiddenException : BusinessLogicException
{
    public ForbiddenException(string? message = null) : base(message)
    {
    }
}