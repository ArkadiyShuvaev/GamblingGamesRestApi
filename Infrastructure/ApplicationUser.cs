using Microsoft.AspNetCore.Identity;

namespace GamblingGamesRestApi.Infrastructure;

/// <summary>
/// Represents a user of the application.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
