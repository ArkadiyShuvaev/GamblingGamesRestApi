using GamblingGamesRestApi.Infrastructure;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Represents members of the UserManager that is created for the favor of unit testing.
/// </summary>
public class UserManagerWrapper : IUserManagerWrapper
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerWrapper(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<ApplicationUser> FindByEmailAsync(string userName)
    {
        return await _userManager.FindByEmailAsync(userName);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
}
