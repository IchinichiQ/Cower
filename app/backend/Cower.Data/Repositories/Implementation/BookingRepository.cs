using Cower.Data.Extensions;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models.Booking;
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
            .Where(x => x.Seat.CoworkingId == coworkingId && 
                        x.BookingDate == date && 
                        x.Status != BookingStatus.Cancelled &&
                        x.Status != BookingStatus.PaymentTimeout &&
                        seatIds.Any(s => s == x.SeatId))
            .Select(x => new BookingTimeSlotDAL(
                x.SeatId,
                x.StartTime,
                x.EndTime))
            .ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<BookingDAL>> GetUserBookings(long userId)
    {
        return await _db.Bookings
            .Where(x => x.UserId == userId)
            .Include(x => x.Payment)
            .Select(x => x.ToBookingDAL())
            .ToArrayAsync();
    }

    public async Task<BookingDAL?> GetBooking(long id)
    {
        return await _db.Bookings
            .Where(x => x.Id == id)
            .Include(x => x.Payment)
            .Select(x => x.ToBookingDAL())
            .FirstOrDefaultAsync();
    }

    public async Task<BookingDAL?> GetBooking(string label)
    {
        return await _db.Bookings
            .Include(x => x.Payment)
            .Where(x => x.Payment != null && x.Payment.Label == label)
            .Select(x => x.ToBookingDAL())
            .FirstOrDefaultAsync();
    }

    public async Task<BookingDAL> AddBooking(BookingDAL booking)
    {
        var entity = booking.ToBookingEntity();
        _db.Bookings.Add(entity);
        await _db.SaveChangesAsync();
        
        return entity.ToBookingDAL();
    }

    public async Task<bool> IsBookingTimeOverlaps(
        long seatId,
        DateOnly bookingDate,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        return await _db.Bookings
            .Where(x => x.SeatId == seatId &&
                        x.BookingDate == bookingDate &&
                        x.Status != BookingStatus.Cancelled &&
                        x.Status != BookingStatus.PaymentTimeout &&
                        (startTime == x.StartTime ||
                         endTime == x.EndTime ||
                         (startTime > x.StartTime && startTime < x.EndTime) ||
                         (endTime >= x.StartTime && endTime < x.EndTime)))
            .AnyAsync();
    }

    public async Task<BookingDAL?> SetBookingStatus(long id, BookingStatus status)
    {
        var entity = await _db.Bookings
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return null;
        }

        entity.Status = status;

        await _db.SaveChangesAsync();

        return entity.ToBookingDAL();
    }
}