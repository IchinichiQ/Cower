using Cower.Data.Models;

namespace Cower.Data.Repositories;

public interface ISeatRepository
{
    Task<CoworkingSeatDal?> GetSeat(long id);
    Task<IReadOnlyCollection<CoworkingSeatDal>> GetSeats();
    Task<CoworkingSeatDal?> UpdateSeat(UpdateCoworkingSeatDal dal);
    Task<bool> DeleteSeat(long id);
    Task<CoworkingSeatDal> AddSeat(AddCoworkingSeatDal dal);
}