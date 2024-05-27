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
}