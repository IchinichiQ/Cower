using Cower.Domain.Models.Booking;
using Cower.Domain.Models.Statistics;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface IBookingService
{
    Task<Booking?> GetBooking(long id, long userId);
    Task<IReadOnlyCollection<Booking>> GetUserBookings(long userId);
    Task<IReadOnlyCollection<Booking>> GetBookings();
    Task<Booking> AddBooking(CreateBookingRequestBL request);
    Task<Booking?> CancelBooking(long id, long userId);
    Task<bool> ProcessPayment(string label, decimal amount);
    Task<int> UpdatePaymentTimeoutStatus();
    Task<int> UpdateInProgressStatus();
    Task<int> UpdateSuccessStatus();
}