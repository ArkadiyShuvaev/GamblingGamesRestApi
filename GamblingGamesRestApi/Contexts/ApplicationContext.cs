using GamblingGamesRestApi.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GamblingGamesRestApi.Contexts;

public class ApplicationIdentityContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options) :
        base(options)
    { }
}
