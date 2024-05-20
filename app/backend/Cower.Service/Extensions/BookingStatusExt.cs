using Cower.Domain.Models.Booking;

namespace Cower.Service.Extensions;

internal static class BookingStatusExt
{
    public static bool CanCancel(this BookingStatus status)
    {
        return status is BookingStatus.AwaitingPayment or BookingStatus.Paid;
    }
}