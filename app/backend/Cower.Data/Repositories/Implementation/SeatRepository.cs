using Cower.Data.Extensions;
using Cower.Data.Models;
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

    public async Task<SeatDAL?> GetSeat(long id)
    {
        return await _db.CoworkingSeats
            .Where(x => x.Id == id)
            .Select(x => x.ToDal())
            .FirstOrDefaultAsync();
    }
}