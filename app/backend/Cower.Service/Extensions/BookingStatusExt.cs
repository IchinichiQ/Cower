using Cower.Domain.Models.Booking;

namespace Cower.Service.Extensions;

internal static class BookingStatusExt
{
    public static bool CanUserCancel(this BookingStatus status)
    {
        return status is BookingStatus.AwaitingPayment or BookingStatus.Paid;
    }
    
    public static bool CanAdminCancel(this BookingStatus status)
    {
        return status is not BookingStatus.Success and not BookingStatus.Cancelled;
    }
}