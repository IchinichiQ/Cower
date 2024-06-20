using Cower.Data.Models.Entities;
using Cower.Domain.Models.Coworking;
using Cower.Service.Models;

namespace Cower.Service.Services;

public interface ICoworkingService
{
    public Task<Coworking?> GetCoworking(long id);
    public Task<IReadOnlyCollection<CoworkingInfo>> GetAllCoworkings();
    public Task<Coworking> CreateCoworking(CreateCoworkingBl request);
    public Task<Coworking?> UpdateCoworking(UpdateCoworkingBl request);
    public Task<bool> DeleteCoworking(long id);
}