using Cower.Domain.Models.Booking;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class BookingExt
{
    public static BookingDto ToBookingDTO(this Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            User = booking.User.ToUserDTO(),
            SeatId = booking.SeatId,
            CreatedAt = booking.CreatedAt.ToString("O"),
            BookingDate = booking.BookingDate.ToString("yyyy-MM-dd"),
            StartTime = booking.StartTime.ToString(),
            EndTime = booking.EndTime.ToString(),
            Status = booking.Status,
            PaymentUrl = booking.Payment?.PaymentUrl,
            PaymentExpireAt = booking.Payment?.ExpireAt.ToString("O"),
            Price = booking.Price,
            SeatNumber = booking.SeatNumber,
            Floor = booking.Floor,
            CoworkingAddress = booking.CoworkingAddress,
            IsDiscountApplied = booking.IsDiscountApplied
        };
    }
}