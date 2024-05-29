using Cower.Data.Extensions;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class SeatRepository : ISeatRepository
{
    private readonly ILogger<SeatRepository> _logger;
    private readonly ApplicationContext _db;

    public SeatRepository(
        ILogger<SeatRepository> logger,
        ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<CoworkingSeatDal?> GetSeat(long id)
    {
        return await _db.CoworkingSeats
            .Where(x => x.Id == id)
            .Include(x => x.Image)
            .Include(x => x.Floor)
            .Select(x => x.ToSeatDal())
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<CoworkingSeatDal>> GetSeats()
    {
        return await _db.CoworkingSeats
            .Include(x => x.Image)
            .Include(x => x.Floor)
            .Select(x => x.ToSeatDal())
            .ToArrayAsync();
    }

    public async Task<CoworkingSeatDal?> UpdateSeat(UpdateCoworkingSeatDal dal)
    {
        var updated = await _db.CoworkingSeats
            .Where(x => x.Id == dal.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.FloorId, x => dal.FloorId ?? x.FloorId)
                .SetProperty(x => x.Number, x => dal.Number ?? x.Number)
                .SetProperty(x => x.Price, x => dal.Price ?? x.Price)
                .SetProperty(x => x.Description, x => dal.Description ?? x.Description)
                .SetProperty(x => x.ImageId, x => dal.ImageId ?? x.ImageId)
                .SetProperty(x => x.X, x => dal.X ?? x.X)
                .SetProperty(x => x.Y, x => dal.Y ?? x.Y)
                .SetProperty(x => x.Width, x => dal.Width ?? x.Width)
                .SetProperty(x => x.Height, x => dal.Height ?? x.Height)
                .SetProperty(x => x.Angle, x => dal.Angle ?? x.Angle));

        if (updated == 0)
        {
            return null;
        }
    
        return await GetSeat(dal.Id);
    }

    public async Task<bool> DeleteSeat(long id)
    {
        var deleted = await _db.CoworkingSeats
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return deleted > 0;
    }

    public async Task<CoworkingSeatDal> AddSeat(AddCoworkingSeatDal dal)
    {
        var entity = new CoworkingSeatEntity
        {
            FloorId = dal.FloorId,
            Price = dal.Price,
            ImageId = dal.ImageId,
            Description = dal.Description,
            Number = dal.Number,
            X = dal.X,
            Y = dal.Y,
            Width = dal.Width,
            Height = dal.Height,
            Angle = dal.Angle
        };

        var seat = await _db.CoworkingSeats.AddAsync(entity);
        await _db.SaveChangesAsync();
        
        await _db.Entry(seat.Entity)
            .Reference(f => f.Image)
            .LoadAsync();

        await _db.Entry(seat.Entity)
            .Reference(f => f.Floor)
            .LoadAsync();

        return seat.Entity.ToSeatDal();
    }
}