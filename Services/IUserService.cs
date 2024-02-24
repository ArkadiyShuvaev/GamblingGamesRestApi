using GamblingGamesRestApi.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace GamblingGamesRestApi.Services;

public interface IUserService
{
    /// <summary>
    /// Creates a new user for the given email and password.
    /// </summary>
    Task<IdentityResult> CreateAsync(string email, string password);

    /// <summary>
    /// Returns the user for the given email.
    /// </summary>
    Task<ApplicationUser> GetAsync(string email);
}