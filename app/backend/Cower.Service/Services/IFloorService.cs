using Cower.Domain.Models.Coworking;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface IFloorService
{
    Task<CoworkingFloor> CreateFloor(CreateFloorBl request);
    Task<CoworkingFloor?> UpdateFloor(UpdateFloorBl request);
    Task<CoworkingFloor?> GetFloor(long id);
    Task<IReadOnlyCollection<CoworkingFloorInfo>> GetFloors();
    Task<bool> DeleteFloor(long id);
    public Task<CoworkingFloorAvailavilityBL?> GetFloorAvailability(
        DateOnly date,
        long floorId);
}