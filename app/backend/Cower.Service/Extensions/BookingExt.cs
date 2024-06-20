using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models.Booking;

namespace Cower.Service.Extensions;

internal static class BookingExt
{
    internal static BookingDal ToBookingDAL(this Booking booking)
    {
        var paymentDAL = booking.Payment != null ? new PaymentDal(
            booking.Payment.Id,
            booking.Payment.BookingId,
            booking.Payment.Label,
            booking.Payment.PaymentUrl,
            booking.Payment.IsCompleted,
            booking.Payment.ExpireAt) : null;

        var userDal = new UserEntity
        {
            Id = booking.User.Id
        };

        return new BookingDal(
            booking.Id,
            userDal,
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
            paymentDAL,
            booking.IsDiscountApplied);
    }
}