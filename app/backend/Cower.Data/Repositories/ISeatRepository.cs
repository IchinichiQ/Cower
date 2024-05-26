using Cower.Data.Models;

namespace Cower.Data.Repositories;

public interface ISeatRepository
{
    Task<CoworkingSeatDal?> GetSeat(long id);
}