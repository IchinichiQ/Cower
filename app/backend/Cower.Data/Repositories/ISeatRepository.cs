using Cower.Data.Models;

namespace Cower.Data.Repositories;

public interface ISeatRepository
{
    Task<SeatDAL?> GetSeat(long id);
}