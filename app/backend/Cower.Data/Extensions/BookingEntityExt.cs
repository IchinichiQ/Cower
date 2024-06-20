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

        var userDal = new UserEntity
        {
            Id = booking.User.Id,
            Email = booking.User.Email,
            Name = booking.User.Name,
            Surname = booking.User.Surname,
            Phone = booking.User.Phone,
            RoleId = booking.User.RoleId,
            Role = booking.User.Role,
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
            booking.isDiscountApplied);
    }
}