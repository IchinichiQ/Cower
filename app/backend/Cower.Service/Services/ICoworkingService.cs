using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface ICoworkingService
{
    public Task<Coworking?> GetCoworking(long id);
    public Task<IReadOnlyCollection<Coworking>> GetAllCoworkings();
    public Task<CoworkingFloor?> GetCoworkingFloor(long coworkingId, int floorNum);
    public Task<CoworkingSeatsAvailavilityResponseBL?> GetSeatsAvailability(
        DateOnly date,
        long coworkingId,
        IReadOnlyCollection<long> seatIds);
}