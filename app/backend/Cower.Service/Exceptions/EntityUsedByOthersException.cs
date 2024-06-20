namespace Cower.Service.Exceptions;

public class EntityUsedByOthersException : BusinessLogicException
{
    public EntityUsedByOthersException(string? message = null) : base(message)
    {
    }
}