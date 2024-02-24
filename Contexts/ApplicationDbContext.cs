using GamblingGamesRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamblingGamesRestApi.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }

    public DbSet<PointEntity> Points { get; set; }
}
