namespace Cower.Service.Exceptions;

public class CoworkingDoesntExistException : BusinessLogicException
{
    public CoworkingDoesntExistException(string? message = null) : base(message)
    {
    }
}