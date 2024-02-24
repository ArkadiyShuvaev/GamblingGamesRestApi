using GamblingGamesRestApi.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace GamblingGamesRestApi.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IdentityResult> CreateAsync(string email, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email, Points = 10000 };

        var result = await _userManager.CreateAsync(user, password);
        _logger.LogInformation("The result of user '{Email}' creation: {Result}", email, result);

        return result;
    }
}
