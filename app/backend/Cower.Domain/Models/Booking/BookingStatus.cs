namespace Cower.Domain.Models.Booking;

public enum BookingStatus
{
    AwaitingPayment,
    Paid,
    InProgress,
    Success,
    Cancelled,
    PaymentTimeout
}