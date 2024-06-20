using Cower.Data.Models;
using Cower.Data.Repositories;
using Cower.Domain.Models;
using Cower.Domain.Models.Coworking;
using Cower.Service.Exceptions;
using Cower.Service.Extensions;
using Cower.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Cower.Service.Services.Implementation;

public class SeatService : ISeatService
{
    private readonly ILogger<SeatService> _logger;
    private readonly ISeatRepository _seatRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageLinkGenerator _imageLinkGenerator;

    public SeatService(
        ILogger<SeatService> logger,
        ISeatRepository seatRepository,
        IImageLinkGenerator imageLinkGenerator,
        IImageRepository imageRepository)
    {
        _logger = logger;
        _seatRepository = seatRepository;
        _imageLinkGenerator = imageLinkGenerator;
        _imageRepository = imageRepository;
    }

    public async Task<CoworkingSeat> CreateSeat(CreateSeatBl request)
    {
        var dal = new AddCoworkingSeatDal(
            request.FloorId,
            request.Number,
            request.Price,
            request.Description,
            request.ImageId,
            request.X,
            request.Y,
            request.Width,
            request.Height,
            request.Angle);

        var imageType = await _imageRepository.GetImageType(request.ImageId);
        if (imageType == null)
        {
            throw new ImageDoesntExistException();
        }
        if (imageType != ImageType.Seat)
        {
            throw new WrongImageType();
        }
        
        CoworkingSeatDal? seat;
        try
        {
            seat = await _seatRepository.AddSeat(dal);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new SeatNumberExistOnFloorException();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        {
            throw new FloorDoesntExistException();
        }

        return seat.ToCoworkingSeat(_imageLinkGenerator);
    }

    public async Task<CoworkingSeat?> UpdateSeat(UpdateSeatBl request)
    {
        var dal = new UpdateCoworkingSeatDal(
            request.Id,
            request.FloorId,
            request.Number,
            request.Price,
            request.Description,
            request.ImageId,
            request.X,
            request.Y,
            request.Width,
            request.Height,
            request.Angle);

        if (request.ImageId != null)
        {
            var imageType = await _imageRepository.GetImageType(request.ImageId.Value);
            if (imageType == null)
            {
                throw new ImageDoesntExistException();
            }
            if (imageType != ImageType.Seat)
            {
                throw new WrongImageType();
            }
        }
        
        CoworkingSeatDal? seat;
        try
        {
            seat = await _seatRepository.UpdateSeat(dal);
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new SeatNumberExistOnFloorException();
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        {
            throw new FloorDoesntExistException();
        }

        return seat?.ToCoworkingSeat(_imageLinkGenerator);
    }

    public async Task<CoworkingSeat?> GetSeat(long id)
    {
        var seat = await _seatRepository.GetSeat(id);

        return seat?.ToCoworkingSeat(_imageLinkGenerator);
    }

    public async Task<IReadOnlyCollection<CoworkingSeat>> GetSeats()
    {
        var seats = await _seatRepository.GetSeats();

        return seats
            .Select(x => x.ToCoworkingSeat(_imageLinkGenerator))
            .ToArray();
    }

    public async Task<bool> DeleteSeat(long id)
    {
        var isDeleted = await _seatRepository.DeleteSeat(id);

        return isDeleted;
    }
}