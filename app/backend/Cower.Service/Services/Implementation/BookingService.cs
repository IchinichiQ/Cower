using Cower.Data.Models;
using Cower.Data.Repositories;
using Cower.Domain.Models.Booking;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class BookingService : IBookingService
{
    private readonly ILogger<BookingService> _logger;
    private readonly IBookingRepository _bookingRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly ICoworkingRepository _coworkingRepository;
    private readonly IYoomoneyService _yoomoneyService;

    public BookingService(
        ILogger<BookingService> logger,
        IBookingRepository bookingRepository,
        ISeatRepository seatRepository,
        ICoworkingRepository coworkingRepository,
        IYoomoneyService yoomoneyService)
    {
        _logger = logger;
        _bookingRepository = bookingRepository;
        _seatRepository = seatRepository;
        _coworkingRepository = coworkingRepository;
        _yoomoneyService = yoomoneyService;
    }
    
    public async Task<Booking?> GetBooking(long id, long userId)
    {
        var bookingDal = await _bookingRepository.GetBooking(id);
        if (bookingDal != null && bookingDal.UserId != userId)
        {
            throw new ForbiddenException();
        }
        
        return bookingDal?.ToBooking();
    }

    public async Task<IReadOnlyCollection<Booking>> GetUserBookings(long userId)
    {
        var bookingDals = await _bookingRepository.GetUserBookings(userId);

        return bookingDals
            .Select(x => x.ToBooking())
            .ToArray();
    }

    public async Task<Booking> AddBooking(CreateBookingRequestBL request)
    {
        ValidateCreateBookingRequest(request);

        var now = DateTimeOffset.UtcNow;
        
        var seat = await _seatRepository.GetSeat(request.SeatId);
        if (seat == null)
        {
            throw new NotFoundException("Рабочее место с таким id не найдено");
        }

        var coworking = await _coworkingRepository.GetCoworking(seat.CoworkingId);
        var workingTime = coworking!.WorkingTimes
            .FirstOrDefault(x => x.DayOfWeek == (int)request.BookingDate.DayOfWeek);
        if (workingTime == null || workingTime.Open > request.StartTime || workingTime.Close < request.EndTime)
        {
            throw new BusinessLogicException("Бронирование должно быть создано в рабочее время коворкинга");
        }

        if (await _bookingRepository.IsBookingTimeOverlaps(
                request.SeatId, request.BookingDate, request.StartTime, request.EndTime))
        {
            throw new BusinessLogicException("Время бронирования пересекается с другим бронированием");
        }

        var bookingPrice = Math.Ceiling((int)(request.EndTime - request.StartTime).TotalMinutes * (seat.Price / 60m));
        var label = Guid.NewGuid().ToString();
        var paymentUrl = await _yoomoneyService.GetPaymentUrl(label, bookingPrice);
        
        var paymentDal = new PaymentDAL(
            -1,
            -1,
            label,
            paymentUrl,
            false,
            now.AddMinutes(10));

        var bookingDal = new BookingDAL(
            -1,
            request.UserId,
            request.SeatId,
            now,
            request.BookingDate,
            request.StartTime,
            request.EndTime,
            BookingStatus.AwaitingPayment,
            bookingPrice,
            seat.Number,
            seat.Floor,
            coworking.Address,
            paymentDal);

        bookingDal = await _bookingRepository.AddBooking(bookingDal);
        return bookingDal.ToBooking();
    }
    
    public async Task<Booking?> CancelBooking(long id, long userId)
    {
        var bookingDal = await _bookingRepository.GetBooking(id);
        if (bookingDal == null)
        {
            return null;
        }
        if (bookingDal.UserId != userId)
        {
            throw new ForbiddenException();
        }

        if (!bookingDal.Status.CanCancel())
        {
            throw new BusinessLogicException($"Нельзя отменить бронирование в статусе {bookingDal.Status}");
        }

        var cancelledBookingDal = await _bookingRepository.SetBookingStatus(id, BookingStatus.Cancelled);

        return cancelledBookingDal?.ToBooking();
    }

    public async Task<bool> ProcessPayment(string label, decimal amount)
    {
        var booking = await _bookingRepository.GetBooking(label);
        if (booking == null)
        {
            return false;
        }
        
        if (Math.Abs(amount - booking.Price) > 1m)
        {
            return false;
        }

        await _bookingRepository.SetBookingStatus(booking.Id, BookingStatus.Paid);
        
        return true;
    }

    private void ValidateCreateBookingRequest(CreateBookingRequestBL request)
    {
        var nowDate = DateOnly.FromDateTime(DateTimeOffset.Now.DateTime);
        var dayDiff = request.BookingDate.DayNumber - nowDate.DayNumber;
        
        if (dayDiff < 0)
        {
            throw new BusinessLogicException("Дата бронирования не может быть в прошлом");
        }
        if (dayDiff > 31)
        {
            throw new BusinessLogicException("Нельзя бронировать более, чем на 31 день вперед");
        }
        
        if (request.EndTime <= request.StartTime)
        {
            throw new BusinessLogicException("Бронирование должно заканчиваться позже, чем начинается");
        }
        if (request.EndTime.Minute % 10 != 0 || request.StartTime.Minute % 10 != 0 )
        {
            throw new BusinessLogicException("Время окончания и начала бронирования должны быть кратны десяти");
        }
    }
}