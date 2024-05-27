namespace Cower.Service.Exceptions;

public class ImageDoesntExistException : BusinessLogicException
{
    public ImageDoesntExistException(string? message = null) : base(message)
    {
    }
}