using GamblingGamesRestApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamblingGamesRestApi.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }

    /// <summary>
    /// Gets or sets a collection of user points.
    /// </summary>
    public DbSet<PointEntity> Points { get; set; }
}
