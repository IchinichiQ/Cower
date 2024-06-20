using Cower.Data.Extensions;
using Cower.Data.Models;
using Cower.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cower.Data.Repositories.Implementation;

public class FloorRepository : IFloorRepository
{
    private readonly ILogger<FloorRepository> _logger;
    private readonly ApplicationContext _db;

    public FloorRepository(
        ILogger<FloorRepository> logger,
        ApplicationContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    public async Task<CoworkingFloorDal> AddFloor(AddCoworkingFloorDal dal)
    {
        var entity = new CoworkingFloorEntity
        {
            CoworkingId = dal.CoworkingId,
            Number = dal.Number,
            ImageId = dal.ImageId
        };

        var floor = await _db.CoworkingFloors.AddAsync(entity);
        await _db.SaveChangesAsync();
        
        await _db.Entry(floor.Entity)
            .Reference(f => f.Image)
            .LoadAsync();

        await _db.Entry(floor.Entity)
            .Collection(f => f.Seats)
            .LoadAsync();
        
        return floor.Entity.ToCoworkingFloorDal();
    }

    public async Task<CoworkingFloorDal?> UpdateFloor(UpdateCoworkingFloorDal dal)
    {
        var updated = await _db.CoworkingFloors
            .Where(x => x.Id == dal.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.CoworkingId, x => dal.CoworkingId ?? x.CoworkingId)
                .SetProperty(x => x.Number, x => dal.Number ?? x.Number)
                .SetProperty(x => x.ImageId,x => dal.ImageId ?? x.ImageId));

        if (updated == 0)
        {
            return null;
        }
        
        return await GetFloor(dal.Id);
    }

    public async Task<CoworkingFloorDal?> GetFloor(long id)
    {
        return await _db.CoworkingFloors
            .Where(x => x.Id == id)
            .Include(x => x.Image)
            .Include(x => x.Seats)
            .ThenInclude(x => x.Image)
            .Select(x => x.ToCoworkingFloorDal())
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<CoworkingFloorInfoDal>> GetAllFloors()
    {
        return await _db.CoworkingFloors
            .Include(x => x.Image)
            .Select(x => x.ToCoworkingFloorInfoDal())
            .ToArrayAsync();
    }

    public async Task<bool> DeleteFloor(long id)
    {
        var deleted = await _db.CoworkingFloors
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return deleted > 0;
    }
}