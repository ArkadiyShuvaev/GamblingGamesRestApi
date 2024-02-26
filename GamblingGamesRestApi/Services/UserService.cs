using GamblingGamesRestApi.Exceptions;
using GamblingGamesRestApi.Infrastructure;
using GamblingGamesRestApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace GamblingGamesRestApi.Services;

/// <summary>
/// Represents members to add a new user.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserManagerWrapper _userManager;
    private readonly IPointRepository _pointRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserManagerWrapper userManager,
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
            // we do not need to normalize the email address because the database is case insensitive
            UserName = email,
            Email = email,
            CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, password);
        _logger.LogInformation("The result of user '{Email}' creation: {Result}", email, result);

        if (result.Succeeded)
        {
            await _pointRepository.UpdateAsync(email, 10000);
        }

        return result;
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var user = await GetAsync(email);
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<ApplicationUser> GetAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"User '{email}' cannot be found.");
        }

        return user;
    }
}
