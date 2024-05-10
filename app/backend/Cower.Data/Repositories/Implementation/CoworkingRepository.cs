using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Cower.Data.Entities;
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

    public async Task<CoworkingEntity?> GetCoworking(long id)
    {
        return await _db.Coworkings
            .Include(x => x.WorkingTimes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CoworkingFloorDAL?> GetCoworkingFloor(long coworkingId, int floorNum)
    {
        var floor = await _db.CoworkingFloorsMedia.FirstOrDefaultAsync(
            x => x.CoworkingId == coworkingId && x.Number == floorNum);

        if (floor == null)
        {
            return null;
        }

        var seats = await _db.CoworkingSeats
            .Where(x => x.CoworkingId == coworkingId && x.Floor == floorNum)
            .ToArrayAsync();

        return new CoworkingFloorDAL(
            floor,
            seats);
    }
}