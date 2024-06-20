using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Repositories;

public interface ICoworkingRepository
{
    public Task<CoworkingDal?> GetCoworking(long id);
    public Task<CoworkingDal> AddCoworking(AddCoworkingDal dal);
    public Task<CoworkingDal?> UpdateCoworking(UpdateCoworkingDal dal);
    public Task<bool> DeleteCoworking(long id);
    public Task<CoworkingDal?> GetCoworkingByFloorId(long floorId);
    public Task<IReadOnlyCollection<CoworkingInfoDal>> GetAllCoworkings();
}