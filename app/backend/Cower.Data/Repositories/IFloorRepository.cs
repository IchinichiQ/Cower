using Cower.Data.Models;

namespace Cower.Data.Repositories;

public interface IFloorRepository
{
    Task<CoworkingFloorDal> AddFloor(AddCoworkingFloorDal dal);
    Task<CoworkingFloorDal?> UpdateFloor(UpdateCoworkingFloorDal dal);
    Task<CoworkingFloorDal?> GetFloor(long id);
    Task<IReadOnlyCollection<CoworkingFloorInfoDal>> GetAllFloors();
    Task<bool> DeleteFloor(long id);
}