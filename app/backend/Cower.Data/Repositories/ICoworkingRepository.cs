using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Data.Entities;

namespace Cower.Data.Repositories;

public interface ICoworkingRepository
{
    public Task<CoworkingEntity?> GetCoworking(long id);
    public Task<CoworkingFloorDAL?> GetCoworkingFloor(long coworkingId, int floorNum);
}