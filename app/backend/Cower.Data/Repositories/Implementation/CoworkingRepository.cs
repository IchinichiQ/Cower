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
}