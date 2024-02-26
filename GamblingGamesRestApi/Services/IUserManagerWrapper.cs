using GamblingGamesRestApi.Infrastructure;
using Microsoft.AspNetCore.Identity;

public interface IUserManagerWrapper
{
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<ApplicationUser> FindByEmailAsync(string userName);
}