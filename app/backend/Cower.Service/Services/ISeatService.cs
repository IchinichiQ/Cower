using Cower.Domain.Models.Coworking;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface ISeatService
{
    Task<CoworkingSeat> CreateSeat(CreateSeatBl request);
    Task<CoworkingSeat?> UpdateSeat(UpdateSeatBl request);
    Task<CoworkingSeat?> GetSeat(long id);
    Task<IReadOnlyCollection<CoworkingSeat>> GetSeats();
    Task<bool> DeleteSeat(long id);
}