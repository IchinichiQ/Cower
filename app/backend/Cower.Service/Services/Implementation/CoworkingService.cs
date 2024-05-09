using Cower.Data.Repositories;
using Cower.Domain.Models.Coworking;
using Cower.Service.Extensions;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class CoworkingService : ICoworkingService
{
    private readonly ILogger<CoworkingService> _logger;
    private readonly ICoworkingRepository _coworkingRepository;

    public CoworkingService(
        ILogger<CoworkingService> logger,
        ICoworkingRepository coworkingRepository)
    {
        _logger = logger;
        _coworkingRepository = coworkingRepository;
    }
    
    public async Task<Coworking?> GetCoworking(long id)
    {
        var coworkingEntity = await _coworkingRepository.GetCoworking(id);

        return coworkingEntity?.ToCoworking();
    }
}