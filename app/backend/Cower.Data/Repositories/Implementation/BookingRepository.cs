using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class BookingRepository : IBookingRepository
{
    private readonly ILogger<BookingRepository> _logger;
    private readonly ApplicationContext _db;

    public BookingRepository(
        ILogger<BookingRepository> logger,
        ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IReadOnlyCollection<BookingTimeSlotDAL>> GetBookingsTimeSlots(
        DateOnly date,
        long coworkingId,
        IReadOnlyCollection<long> seatIds)
    {
        return await _db.Bookings
            .Where(x => x.Seat.CoworkingId == coworkingId && x.BookingDate == date && seatIds.Any(s => s == x.SeatId))
            .Select(x => new BookingTimeSlotDAL(
                x.SeatId,
                x.StartTime,
                x.EndTime))
            .ToArrayAsync();
    }
}