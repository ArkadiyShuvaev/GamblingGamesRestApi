using Microsoft.AspNetCore.Identity;

namespace GamblingGamesRestApi.Services;

public interface IUserService
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    Task<IdentityResult> CreateAsync(string email, string password);
}