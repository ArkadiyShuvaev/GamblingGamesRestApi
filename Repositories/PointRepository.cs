using GamblingGamesRestApi.Contexts;
using GamblingGamesRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamblingGamesRestApi.Repositories;

public class PointRepository : IPointRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PointRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(string email, int points)
    {
        var point = await _dbContext.Points.FirstOrDefaultAsync(p => p.Email == email);
        if (point == null)
        {
            point = new PointEntity { Email = email, Value = points };
            _dbContext.Points.Add(point);
        }
        else
        {
            point.Value += points;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetAsync(string email)
    {
        var point = await _dbContext.Points.FirstOrDefaultAsync(p => p.Email == email);
        return point?.Value ?? 0;
    }
}
