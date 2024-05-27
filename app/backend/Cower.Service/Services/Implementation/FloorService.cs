using Cower.Data.Models;
using Cower.Data.Repositories;
using Cower.Domain.Models;
using Cower.Domain.Models.Coworking;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Cower.Service.Services.Implementation;

public class FloorService : IFloorService
{
    private readonly ILogger<FloorService> _logger;
    private readonly IFloorRepository _floorRepository;
    private readonly ICoworkingRepository _coworkingRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageLinkGenerator _imageLinkGenerator;

    public FloorService(
        ILogger<FloorService> logger,
        IFloorRepository floorRepository,
        ICoworkingRepository coworkingRepository,
        IBookingRepository bookingRepository,
        IImageRepository imageRepository,
        IImageLinkGenerator imageLinkGenerator)
    {
        _logger = logger;
        _floorRepository = floorRepository;
        _coworkingRepository = coworkingRepository;
        _bookingRepository = bookingRepository;
        _imageRepository = imageRepository;
        _imageLinkGenerator = imageLinkGenerator;
    }

    public async Task<CoworkingFloor> CreateFloor(CreateFloorBl request)
    {
        var dal = new AddCoworkingFloorDal(
            request.CoworkingId,
            request.ImageId,
            request.Number);
        
        var imageType = await _imageRepository.GetImageType(request.ImageId);
        if (imageType == null)
        {
            throw new ImageDoesntExistException();
        }
        if (imageType != ImageType.Floor)
        {
            throw new WrongImageType();
        }
        
        CoworkingFloorDal? floor;
        try
        {
            floor = await _floorRepository.AddFloor(dal);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new FloorNumberExistInCoworkingException();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        {
            throw new CoworkingDoesntExistException();
        }
        
        return floor.ToCoworkingFloor(_imageLinkGenerator);
    }

    public async Task<CoworkingFloor?> UpdateFloor(UpdateFloorBl request)
    {
        var dal = new UpdateCoworkingFloorDal(
            request.Id,
            request.CoworkingId,
            request.ImageId,
            request.Number);

        if (request.ImageId != null)
        {
            var imageType = await _imageRepository.GetImageType(request.ImageId.Value);
            if (imageType == null)
            {
                throw new ImageDoesntExistException();
            }
            if (imageType != ImageType.Floor)
            {
                throw new WrongImageType();
            }
        }
        
        CoworkingFloorDal? floor;
        try
        {
            floor = await _floorRepository.UpdateFloor(dal);
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new FloorNumberExistInCoworkingException();
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        {
            throw new CoworkingDoesntExistException();
        }

        return floor?.ToCoworkingFloor(_imageLinkGenerator);
    }

    public async Task<CoworkingFloor?> GetFloor(long id)
    {
        var floor = await _floorRepository.GetFloor(id);

        return floor?.ToCoworkingFloor(_imageLinkGenerator);
    }

    public async Task<IReadOnlyCollection<CoworkingFloorInfo>> GetFloors()
    {
        var floors = await _floorRepository.GetAllFloors();

        return floors
            .Select(x => x.ToCoworkingFloorInfo(_imageLinkGenerator))
            .ToArray();
    }

    public async Task<bool> DeleteFloor(long id)
    {
        var isDeleted = await _floorRepository.DeleteFloor(id);

        return isDeleted;
    }

    public async Task<CoworkingFloorAvailavilityBL?> GetFloorAvailability(DateOnly date, long floorId)
    {
        var floor = await _floorRepository.GetFloor(floorId);
        if (floor == null)
        {
            return null;
        }

        var coworking = await _coworkingRepository.GetCoworking(floor.CoworkingId);
        if (coworking == null)
        {
            return null;
        }
        
        var workingTime = coworking.WorkingTimes
            .FirstOrDefault(x => x.DayOfWeek == (int)date.DayOfWeek);
        
        var availability = floor.Seats.ToDictionary(
            x => x.Id,
            x =>
            {
                var list = new LinkedList<CoworkingSeatsAvailavilityTimeSlotBL>();
                if (workingTime != null)
                {
                    list.AddFirst(new CoworkingSeatsAvailavilityTimeSlotBL(workingTime.Open, workingTime.Close));
                }

                return list;
            });
        
        var timeSlots = await _bookingRepository.GetBookingsTimeSlots(date, floorId);
        var groupedTimeSlots = timeSlots.GroupBy(x => x.SeatId);
        foreach (var groupBySeat in groupedTimeSlots)
        {
            foreach (var bookingSlot in groupBySeat)
            {
                var collision = availability[groupBySeat.Key]
                    .Nodes()
                    .FirstOrDefault(x => x.ValueRef.To > bookingSlot.StartTime);

                var firstPart = new LinkedListNode<CoworkingSeatsAvailavilityTimeSlotBL>(
                    new CoworkingSeatsAvailavilityTimeSlotBL(collision.ValueRef.From, bookingSlot.StartTime));
                var secondPart = new LinkedListNode<CoworkingSeatsAvailavilityTimeSlotBL>(
                    new CoworkingSeatsAvailavilityTimeSlotBL(bookingSlot.EndTime, collision.ValueRef.To));

                if (firstPart.ValueRef.From != firstPart.ValueRef.To)
                {
                    availability[groupBySeat.Key].AddAfter(collision, firstPart);
                }
                if (secondPart.ValueRef.From != secondPart.ValueRef.To)
                {
                    availability[groupBySeat.Key].AddAfter(collision, secondPart);
                }
                
                availability[groupBySeat.Key].Remove(collision);
            }
        }

        var dict = availability.ToDictionary(
            x => x.Key, 
            x => (IReadOnlyCollection<CoworkingSeatsAvailavilityTimeSlotBL>)x.Value.OrderBy(x => x.From).ToList());
        
        return new CoworkingFloorAvailavilityBL(date, dict);
    }
}