using Cower.Domain.Models.Coworking;

namespace Cower.Service.Services;

public interface ICoworkingService
{
    public Task<Coworking?> GetCoworking(long id);
    public Task<CoworkingFloor?> GetCoworkingFloor(long coworkingId, int floorNum);
}