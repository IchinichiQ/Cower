namespace Cower.Service.Exceptions;

public class NotFoundException : BusinessLogicException
{
    public NotFoundException(string? message = null) : base(message)
    {
    }
}