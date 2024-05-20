using Cower.Data.Models;
using Cower.Domain.Models.Booking;

namespace Cower.Service.Extensions;

internal static class BookingDalExt
{
    internal static Booking ToBooking(this BookingDAL bookingDal)
    {
        var payment = bookingDal.Payment != null ? new Payment(
            bookingDal.Payment.Id,
            bookingDal.Payment.BookingId,
            bookingDal.Payment.Label,
            bookingDal.Payment.PaymentUrl,
            bookingDal.Payment.IsCompleted,
            bookingDal.Payment.ExpireAt) : null;

        return new Booking(
            bookingDal.Id,
            bookingDal.UserId,
            bookingDal.SeatId,
            bookingDal.CreatedAt,
            bookingDal.BookingDate,
            bookingDal.StartTime,
            bookingDal.EndTime,
            bookingDal.Status,
            bookingDal.Price,
            bookingDal.SeatNumber,
            bookingDal.Floor,
            bookingDal.CoworkingAddress,
            payment);
    }
}