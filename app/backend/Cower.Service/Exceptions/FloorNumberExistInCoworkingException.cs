namespace Cower.Service.Exceptions;

public class FloorNumberExistInCoworkingException : BusinessLogicException
{
    public FloorNumberExistInCoworkingException(string? message = null) : base(message)
    {
    }
}