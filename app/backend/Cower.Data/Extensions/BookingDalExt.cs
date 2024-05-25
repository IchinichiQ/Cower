using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

public static class BookingDalExt
{
    public static BookingEntity ToBookingEntity(this BookingDAL bookingDal)
    {
        var paymentEntity = bookingDal.Payment != null ? new PaymentEntity
        {
            Label = bookingDal.Payment.Label,
            IsCompleted = bookingDal.Payment.IsCompleted,
            PaymentUrl = bookingDal.Payment.PaymentUrl,
            ExpireAt = bookingDal.Payment.ExpireAt
        } : null;

        return new BookingEntity
        {
            UserId = bookingDal.UserId,
            SeatId = bookingDal.SeatId,
            CreatedAt = bookingDal.CreatedAt,
            BookingDate = bookingDal.BookingDate,
            StartTime = bookingDal.StartTime,
            EndTime = bookingDal.EndTime,
            Status = bookingDal.Status,
            Price = bookingDal.Price,
            SeatNumber = bookingDal.SeatNumber,
            Floor = bookingDal.Floor,
            CoworkingAddress = bookingDal.CoworkingAddress,
            Payment = paymentEntity
        };
    }
}