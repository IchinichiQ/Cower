using Cower.Data.Entities;

namespace Cower.Data.Repositories;

public interface ICoworkingRepository
{
    public Task<CoworkingEntity?> GetCoworking(long id);
}