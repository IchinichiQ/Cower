using System.Collections.ObjectModel;
using Cower.Data.Repositories;
using Cower.Domain.Models.Coworking;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class CoworkingService : ICoworkingService
{
    private readonly ILogger<CoworkingService> _logger;
    private readonly ICoworkingRepository _coworkingRepository;
    private readonly IBookingRepository _bookingRepository;

    public CoworkingService(
        ILogger<CoworkingService> logger,
        ICoworkingRepository coworkingRepository,
        IBookingRepository bookingRepository)
    {
        _logger = logger;
        _coworkingRepository = coworkingRepository;
        _bookingRepository = bookingRepository;
    }
    
    public async Task<Coworking?> GetCoworking(long id)
    {
        var coworkingEntity = await _coworkingRepository.GetCoworking(id);

        return coworkingEntity?.ToCoworking();
    }

    public async Task<IReadOnlyCollection<Coworking>> GetAllCoworkings()
    {
        var coworkingEntities = await _coworkingRepository.GetAllCoworkings();

        return coworkingEntities
            .Select(x => x.ToCoworking())
            .ToArray();
    }

    public async Task<CoworkingFloor?> GetCoworkingFloor(long coworkingId, int floorNum)
    {
        var floorDal = await _coworkingRepository.GetCoworkingFloor(coworkingId, floorNum);

        return floorDal?.ToCoworkingFloor();
    }

    public async Task<CoworkingSeatsAvailavilityResponseBL?> GetSeatsAvailability(
        DateOnly date,
        long coworkingId,
        IReadOnlyCollection<long> seatIds)
    {
        var coworking = await _coworkingRepository.GetCoworking(coworkingId);
        if (coworking == null)
        {
            return null;
        }

        var workingTime = coworking.WorkingTimes
            .FirstOrDefault(x => x.DayOfWeek == (int)date.DayOfWeek);
        var availability = new Dictionary<long, LinkedList<CoworkingSeatsAvailavilityTimeSlotBL>>();
        foreach (var seatId in seatIds)
        {
            availability[seatId] = new LinkedList<CoworkingSeatsAvailavilityTimeSlotBL>();
            if (workingTime != null)
            {
                availability[seatId]
                    .AddFirst(new CoworkingSeatsAvailavilityTimeSlotBL(workingTime.Open, workingTime.Close));
            }
        }
        
        var timeSlots = await _bookingRepository.GetBookingsTimeSlots(date, coworkingId, seatIds);
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
        
        return new CoworkingSeatsAvailavilityResponseBL(date, dict);
    }
}