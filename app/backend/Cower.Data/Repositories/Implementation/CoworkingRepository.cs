using Cower.Data.Extensions;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class CoworkingRepository : ICoworkingRepository
{
    private readonly ILogger<CoworkingRepository> _logger;
    private readonly ApplicationContext _db;

    public CoworkingRepository(
        ILogger<CoworkingRepository> logger,
        ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<CoworkingDal?> GetCoworking(long id)
    {
        return await _db.Coworkings
            .Include(x => x.Floors)
            .ThenInclude(floor => floor.Seats)
            .ThenInclude(seat => seat.Image)
            .Include(x => x.Floors)
            .ThenInclude(floor => floor.Image)
            .Include(x => x.WorkingTimes)
            .Where(x => x.Id == id)
            .Select(x => x.ToCoworkingDal())
            .FirstOrDefaultAsync();
    }

    public async Task<CoworkingDal?> GetCoworkingByFloorId(long floorId)
    {
        return await _db.Coworkings
            .Include(x => x.Floors)
            .ThenInclude(floor => floor.Seats)
            .ThenInclude(seat => seat.Image)
            .Include(x => x.Floors)
            .ThenInclude(floor => floor.Image)
            .Include(x => x.WorkingTimes)
            .Where(x => x.Floors.Any(floor => floor.Id == floorId))
            .Select(x => x.ToCoworkingDal())
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<CoworkingInfoDal>> GetAllCoworkings()
    {
        return await _db.Coworkings
            .Include(x => x.Floors)
            .ThenInclude(x => x.Image)
            .Include(x => x.WorkingTimes)
            .Select(x => x.ToCoworkingInfoDal())
            .ToArrayAsync();
    }

    public async Task<CoworkingDal> AddCoworking(AddCoworkingDal dal)
    {
        var entity = new CoworkingEntity
        {
            Address = dal.Address,
            WorkingTimes = dal.WorkingTimes.Select(x => new CoworkingWorkingTimeEntity
            {
                DayOfWeek = x.DayOfWeek,
                Open = x.Open,
                Close = x.Close
            }).ToArray()
        };
        
        var coworking = await _db.Coworkings.AddAsync(entity);
        await _db.SaveChangesAsync();
        
        await _db.Entry(coworking.Entity)
            .Collection(x => x.WorkingTimes)
            .LoadAsync();

        await _db.Entry(coworking.Entity)
            .Collection(x => x.Floors)
            .LoadAsync();

        return coworking.Entity.ToCoworkingDal();
    }

    public async Task<CoworkingDal?> UpdateCoworking(UpdateCoworkingDal dal)
    {
        ICollection<CoworkingWorkingTimeEntity>? workingTimeEntities = dal.WorkingTimes?
            .Select(x => new CoworkingWorkingTimeEntity
            {
                DayOfWeek = x.DayOfWeek,
                Open = x.Open,
                Close = x.Close
            }).ToArray();

        var coworkingEntity = await _db.Coworkings
            .Where(x => x.Id == dal.Id)
            .Include(coworkingEntity => coworkingEntity.WorkingTimes)
            .FirstOrDefaultAsync();

        if (coworkingEntity == null)
        {
            return null;
        }

        coworkingEntity.Address = dal.Address ?? coworkingEntity.Address;
        coworkingEntity.WorkingTimes = workingTimeEntities ?? coworkingEntity.WorkingTimes;

        await _db.SaveChangesAsync();
        
        return await GetCoworking(dal.Id);
    }

    public async Task<bool> DeleteCoworking(long id)
    {
        var deleted = await _db.Coworkings
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return deleted > 0;
    }
}