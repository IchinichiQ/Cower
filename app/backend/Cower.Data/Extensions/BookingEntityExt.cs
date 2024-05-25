using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class BookingEntityExt
{
    public static BookingDAL ToBookingDAL(this BookingEntity booking)
    {
        var paymentDAL = booking.Payment != null ? new PaymentDAL(
            booking.Payment.Id,
            booking.Payment.BookingId,
            booking.Payment.Label,
            booking.Payment.PaymentUrl,
            booking.Payment.IsCompleted,
            booking.Payment.ExpireAt) : null;

        return new BookingDAL(
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