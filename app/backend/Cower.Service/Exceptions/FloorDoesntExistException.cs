namespace Cower.Service.Exceptions;

public class FloorDoesntExistException : BusinessLogicException
{
    public FloorDoesntExistException(string? message = null) : base(message)
    {
    }
}