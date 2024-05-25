using Cower.Domain.Models.Booking;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

internal static class BookingExt
{
    public static BookingDTO ToBookingDTO(this Booking booking)
    {
        return new BookingDTO
        {
            Id = booking.Id,
            UserId = booking.UserId,
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
            CoworkingAddress = booking.CoworkingAddress
        };
    }
}