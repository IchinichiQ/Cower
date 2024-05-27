namespace Cower.Service.Exceptions;

public class WrongImageType : BusinessLogicException
{
    public WrongImageType(string? message = null) : base(message)
    {
    }
}