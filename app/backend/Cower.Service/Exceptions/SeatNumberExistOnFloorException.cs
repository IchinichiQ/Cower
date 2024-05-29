namespace Cower.Service.Exceptions;

public class SeatNumberExistOnFloorException : BusinessLogicException
{
    public SeatNumberExistOnFloorException(string? message = null) : base(message)
    {
    }
}