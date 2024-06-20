using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Data.Repositories;
using Cower.Domain.Models;
using Cower.Domain.Models.Booking;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class BookingService : IBookingService
{
    public const decimal DISCOUNT_COEFFICIENT = 0.9m;
    
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
        if (bookingDal != null && bookingDal.User.Id != userId)
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

    public async Task<IReadOnlyCollection<Booking>> GetBookings()
    {
        var bookingDals = await _bookingRepository.GetBookings();

        return bookingDals
            .Select(x => x.ToBooking())
            .ToArray();
    }

    public async Task<Booking> AddBooking(CreateBookingRequestBL request)
    {
        ValidateCreateBookingRequest(request);

        var now = DateTimeOffset.UtcNow;
        var userIsAdmin = request.UserRole == AppRoleNames.Admin;
        
        var seat = await _seatRepository.GetSeat(request.SeatId);
        if (seat == null)
        {
            throw new NotFoundException("Рабочее место с таким id не найдено");
        }

        var coworking = await _coworkingRepository.GetCoworkingByFloorId(seat.FloorId);
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

        var bookedHours = (decimal)(request.EndTime - request.StartTime).TotalHours;
        var bookingPrice = Math.Ceiling(seat.Price * bookedHours);
        bookingPrice = request.ApplyDiscount ? Math.Floor(bookingPrice * DISCOUNT_COEFFICIENT) : bookingPrice;
        
        var label = Guid.NewGuid().ToString();
        var paymentUrl = await _yoomoneyService.GetPaymentUrl(label, bookingPrice);
        
        var paymentDal = new PaymentDal(
            -1,
            -1,
            label,
            paymentUrl,
            userIsAdmin,
            now.AddMinutes(10));

        var bookingStatus = userIsAdmin ? BookingStatus.Paid : BookingStatus.AwaitingPayment;
        
        var bookingDal = new BookingDal(
            -1,
            new UserEntity
            {
                Id = request.UserId
            },
            request.SeatId,
            now,
            request.BookingDate,
            request.StartTime,
            request.EndTime,
            bookingStatus,
            bookingPrice,
            seat.Number,
            seat.FloorNumber,
            coworking.Address,
            paymentDal,
            request.ApplyDiscount);

        bookingDal = await _bookingRepository.AddBooking(bookingDal);
        return bookingDal.ToBooking();
    }
    
    public async Task<Booking?> CancelBooking(long id, long userId, string userRole)
    {
        var bookingDal = await _bookingRepository.GetBooking(id);
        if (bookingDal == null)
        {
            return null;
        }

        var userIsAdmin = userRole == AppRoleNames.Admin;
        
        if (!userIsAdmin && bookingDal.User.Id != userId)
        {
            throw new ForbiddenException();
        }

        var canCancel = userIsAdmin ?
            bookingDal.Status.CanAdminCancel() :
            bookingDal.Status.CanUserCancel();
        
        if (!canCancel)
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

        if (booking.Status != BookingStatus.AwaitingPayment)
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

    public async Task<int> UpdatePaymentTimeoutStatus()
    {
        return await _bookingRepository.SetPaymentTimeoutStatus();
    }

    public async Task<int> UpdateInProgressStatus()
    {
        return await _bookingRepository.SetInProgressStatus();
    }

    public async Task<int> UpdateSuccessStatus()
    {
        return await _bookingRepository.SetSuccessBookingStatus();
    }

    private void ValidateCreateBookingRequest(CreateBookingRequestBL request)
    {
        var nowTime = TimeOnly.FromDateTime(DateTimeOffset.Now.DateTime);
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
        
        if (dayDiff == 0 && (request.StartTime < nowTime || request.EndTime < nowTime))
        {
            throw new BusinessLogicException("Время начала или окончания бронирования не может быть в прошлом");
        }
    }
}