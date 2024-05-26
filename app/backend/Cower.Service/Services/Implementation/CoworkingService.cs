using System.Collections.ObjectModel;
using Cower.Data.Repositories;
using Cower.Domain.Models.Coworking;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class CoworkingService : ICoworkingService
{
    private readonly ILogger<CoworkingService> _logger;
    private readonly ICoworkingRepository _coworkingRepository;
    private readonly IBookingRepository _bookingRepository;

    public CoworkingService(
        ILogger<CoworkingService> logger,
        ICoworkingRepository coworkingRepository,
        IBookingRepository bookingRepository)
    {
        _logger = logger;
        _coworkingRepository = coworkingRepository;
        _bookingRepository = bookingRepository;
    }
    
    public async Task<Coworking?> GetCoworking(long id)
    {
        var dal = await _coworkingRepository.GetCoworking(id);

        return dal?.ToCoworking();
    }

    public async Task<IReadOnlyCollection<CoworkingInfo>> GetAllCoworkings()
    {
        var infoDals = await _coworkingRepository.GetAllCoworkings();

        return infoDals
            .Select(x => x.ToCoworkingInfo())
            .ToArray();
    }
}