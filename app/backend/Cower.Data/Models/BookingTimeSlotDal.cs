namespace Cower.Data.Models;

public sealed record BookingTimeSlotDal(
    long SeatId,
    TimeOnly StartTime,
    TimeOnly EndTime);