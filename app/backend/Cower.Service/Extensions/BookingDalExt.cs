using Cower.Data.Models;
using Cower.Domain.Models.Booking;
using Cower.Service.Services.Implementation;

namespace Cower.Service.Extensions;

internal static class BookingDalExt
{
    internal static Booking ToBooking(this BookingDal bookingDal)
    {
        var payment = bookingDal.Payment != null ? new Payment(
            bookingDal.Payment.Id,
            bookingDal.Payment.BookingId,
            bookingDal.Payment.Label,
            bookingDal.Payment.PaymentUrl,
            bookingDal.Payment.IsCompleted,
            bookingDal.Payment.ExpireAt) : null;

        decimal? initialPrice = bookingDal.IsDiscountApplied
            ? Math.Ceiling(bookingDal.Price / BookingService.DISCOUNT_COEFFICIENT)
            : null;
        
        return new Booking(
            bookingDal.Id,
            bookingDal.User.ToUser(),
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
            payment,
            bookingDal.IsDiscountApplied,
            initialPrice);
    }
}