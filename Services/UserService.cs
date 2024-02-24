using GamblingGamesRestApi.Infrastructure;
using GamblingGamesRestApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace GamblingGamesRestApi.Services;

/// <summary>
/// Represents members to add a new user.
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPointRepository _pointRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager,
        IPointRepository pointRepository,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _pointRepository = pointRepository;
        _logger = logger;
    }

    public async Task<IdentityResult> CreateAsync(string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, password);
        _logger.LogInformation("The result of user '{Email}' creation: {Result}", email, result);

        if (result.Succeeded)
        {
            await _pointRepository.AddAsync(email, 10000);
        }

        return result;
    }
}
