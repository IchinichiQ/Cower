using Cower.Data.Extensions;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Domain.Models.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class BookingRepository : IBookingRepository
{
    private readonly TimeSpan OFFSET = TimeSpan.FromHours(3);
    
    private readonly ILogger<BookingRepository> _logger;
    private readonly ApplicationContext _db;

    public BookingRepository(
        ILogger<BookingRepository> logger,
        ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    public async Task<IReadOnlyCollection<BookingTimeSlotDal>> GetBookingsTimeSlots(
        DateOnly date,
        long floorId)
    {
        return await _db.Bookings
            .Include(x => x.Seat)
            .Where(x => x.Seat != null &&
                        x.Seat.FloorId == floorId && 
                        x.BookingDate == date && 
                        x.Status != BookingStatus.Cancelled &&
                        x.Status != BookingStatus.PaymentTimeout)
            .Select(x => new BookingTimeSlotDal(
                x.SeatId,
                x.StartTime,
                x.EndTime))
            .ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<BookingDal>> GetUserBookings(long userId)
    {
        return await _db.Bookings
            .Include(x => x.User)
            .ThenInclude(x => x.Role)
            .Where(x => x.UserId == userId)
            .Include(x => x.Payment)
            .Select(x => x.ToBookingDAL())
            .ToArrayAsync();
    }

    public async Task<BookingDal?> GetBooking(long id)
    {
        return await _db.Bookings
            .Where(x => x.Id == id)
            .Include(x => x.Payment)
            .Include(x => x.User)
            .ThenInclude(x => x.Role)
            .Select(x => x.ToBookingDAL())
            .FirstOrDefaultAsync();
    }

    public async Task<BookingDal?> GetBooking(string label)
    {
        return await _db.Bookings
            .Include(x => x.Payment)
            .Where(x => x.Payment != null && x.Payment.Label == label)
            .Select(x => x.ToBookingDAL())
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<BookingDal>> GetBookings()
    {
        return await _db.Bookings
            .Include(x => x.Payment)
            .Include(x => x.User)
            .ThenInclude(x => x.Role)
            .Select(x => x.ToBookingDAL())
            .ToArrayAsync();
    }

    public async Task<BookingDal> AddBooking(BookingDal booking)
    {
        var entity = booking.ToBookingEntity();
        _db.Bookings.Add(entity);
        await _db.SaveChangesAsync();

        return await GetBooking(entity.Id);
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

    public async Task<BookingDal?> SetBookingStatus(long id, BookingStatus status)
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

    public async Task<int> SetPaymentTimeoutStatus()
    {
        var now = DateTimeOffset.UtcNow;
        var updated = await _db.Bookings
            .Include(x => x.Payment)
            .Where(x => 
                x.Status == BookingStatus.AwaitingPayment &&
                x.Payment != null &&
                x.Payment.ExpireAt <= now)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(entity => entity.Status, BookingStatus.PaymentTimeout));

        return updated;
    }

    public async Task<int> SetInProgressStatus()
    {
        var now = DateTimeOffset.Now.ToOffset(OFFSET);
        
        var updated = await _db.Bookings
            .Where(x => 
                x.Status == BookingStatus.Paid &&
                x.BookingDate <= DateOnly.FromDateTime(now.DateTime) &&
                x.StartTime <= TimeOnly.FromDateTime(now.DateTime) &&
                x.EndTime > TimeOnly.FromDateTime(now.DateTime))
            .ExecuteUpdateAsync(x =>
                x.SetProperty(entity => entity.Status, BookingStatus.InProgress));

        return updated;
    }

    public async Task<int> SetSuccessBookingStatus()
    {
        var now = DateTimeOffset.Now.ToOffset(OFFSET);

        var updated = await _db.Bookings
            .Where(x => 
                (x.Status == BookingStatus.InProgress || x.Status == BookingStatus.Paid) &&
                x.BookingDate <= DateOnly.FromDateTime(now.DateTime) &&
                x.EndTime <= TimeOnly.FromDateTime(now.DateTime))
            .ExecuteUpdateAsync(x =>
                x.SetProperty(entity => entity.Status, BookingStatus.Success));

        return updated;
    }

    public async Task<long> GetSuccessfulBookingsCount(DateOnly startDate, DateOnly endDate)
    {
        var startDateTime = new DateTimeOffset(
            startDate.Year,startDate.Month, startDate.Day, 0, 0, 0, TimeSpan.FromHours(0));
        var endDateTime = new DateTimeOffset(
            endDate.Year, endDate.Month, endDate.Day, 0, 0, 0, TimeSpan.FromHours(0));

        return await _db.Bookings
            .Where(x => x.Status == BookingStatus.Success &&
                        x.CreatedAt >= startDateTime &&
                        x.CreatedAt <= endDateTime)
            .LongCountAsync();
    }

    public async Task<decimal> GetSuccessfulBookingsPrice(DateOnly startDate, DateOnly endDate)
    {
        var startDateTime = new DateTimeOffset(
            startDate.Year,startDate.Month, startDate.Day, 0, 0, 0, TimeSpan.FromHours(0));
        var endDateTime = new DateTimeOffset(
            endDate.Year, endDate.Month, endDate.Day, 0, 0, 0, TimeSpan.FromHours(0));

        return await _db.Bookings
            .Where(x => x.Status == BookingStatus.Success &&
                        x.CreatedAt >= startDateTime &&
                        x.CreatedAt <= endDateTime)
            .SumAsync(x => x.Price);
    }
}