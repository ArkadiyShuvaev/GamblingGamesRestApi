using Microsoft.AspNetCore.Identity;

namespace GamblingGamesRestApi.Infrastructure;

public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the number of points the player has.
    /// The player has a starting account of 10,000 points and can wager on a prediction that they will either win or lose.
    /// </summary>
    public int Points { get; set; }
}
