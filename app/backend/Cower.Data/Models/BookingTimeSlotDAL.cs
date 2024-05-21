namespace Cower.Data.Models;

public sealed record BookingTimeSlotDAL(
    long SeatId,
    TimeOnly StartTime,
    TimeOnly EndTime);