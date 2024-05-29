using System.Collections.ObjectModel;
using Cower.Data.Models;
using Cower.Data.Repositories;
using Cower.Domain.Models.Coworking;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Cower.Service.Services.Implementation;

public class CoworkingService : ICoworkingService
{
    private readonly ILogger<CoworkingService> _logger;
    private readonly ICoworkingRepository _coworkingRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IImageLinkGenerator _imageLinkGenerator;

    public CoworkingService(
        ILogger<CoworkingService> logger,
        ICoworkingRepository coworkingRepository,
        IBookingRepository bookingRepository,
        IImageLinkGenerator imageLinkGenerator)
    {
        _logger = logger;
        _coworkingRepository = coworkingRepository;
        _bookingRepository = bookingRepository;
        _imageLinkGenerator = imageLinkGenerator;
    }
    
    public async Task<Coworking?> GetCoworking(long id)
    {
        var dal = await _coworkingRepository.GetCoworking(id);

        return dal?.ToCoworking(_imageLinkGenerator);
    }

    public async Task<IReadOnlyCollection<CoworkingInfo>> GetAllCoworkings()
    {
        var infoDals = await _coworkingRepository.GetAllCoworkings();

        return infoDals
            .Select(x => x.ToCoworkingInfo(_imageLinkGenerator))
            .ToArray();
    }

    public async Task<Coworking> CreateCoworking(CreateCoworkingBl request)
    {
        var dal = new AddCoworkingDal(
            request.Address,
            request.WorkingTimes
                .Select(x => new AddCoworkingWorkingTimeDal(
                    (int)x.DayOfWeek,
                    x.Open,
                    x.Close))
                .ToArray());
        
        var coworkingDal = await _coworkingRepository.AddCoworking(dal);

        return coworkingDal.ToCoworking(_imageLinkGenerator);
    }

    public async Task<Coworking?> UpdateCoworking(UpdateCoworkingBl request)
    {
        var dal = new UpdateCoworkingDal(
            request.Id,
            request.Address,
            request.WorkingTimes?
                .Select(x => new UpdateCoworkingWorkingTimeDal(
                    (int)x.DayOfWeek,
                    x.Open,
                    x.Close))
                .ToArray());
        
        var coworkingDal = await _coworkingRepository.UpdateCoworking(dal);

        return coworkingDal?.ToCoworking(_imageLinkGenerator);
    }

    public async Task<bool> DeleteCoworking(long id)
    {
        var isDeleted = await _coworkingRepository.DeleteCoworking(id);

        return isDeleted;
    }
}