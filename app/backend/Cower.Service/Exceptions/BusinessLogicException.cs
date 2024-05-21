namespace Cower.Service.Exceptions;

public class BusinessLogicException : Exception
{
    public BusinessLogicException(string? message) : base(message)
    {
    }
}