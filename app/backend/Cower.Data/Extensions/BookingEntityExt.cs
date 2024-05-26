using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class BookingEntityExt
{
    public static BookingDal ToBookingDAL(this BookingEntity booking)
    {
        var paymentDAL = booking.Payment != null ? new PaymentDal(
            booking.Payment.Id,
            booking.Payment.BookingId,
            booking.Payment.Label,
            booking.Payment.PaymentUrl,
            booking.Payment.IsCompleted,
            booking.Payment.ExpireAt) : null;

        return new BookingDal(
            booking.Id,
            booking.UserId,
            booking.SeatId,
            booking.CreatedAt,
            booking.BookingDate,
            booking.StartTime,
            booking.EndTime,
            booking.Status,
            booking.Price,
            booking.SeatNumber,
            booking.Floor,
            booking.CoworkingAddress,
            paymentDAL);
    }
}