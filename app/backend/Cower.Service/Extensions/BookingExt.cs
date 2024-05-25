using Cower.Data.Models;
using Cower.Domain.Models.Booking;

namespace Cower.Service.Extensions;

internal static class BookingExt
{
    internal static BookingDAL ToBookingDAL(this Booking booking)
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