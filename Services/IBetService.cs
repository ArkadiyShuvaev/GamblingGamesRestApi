namespace GamblingGamesRestApi.Services;

public interface IBetService
{
    /// <summary>
    /// Creates a bet for the specified user.
    /// </summary>
    Task PlaceBetAsync(string email, int number, int points);
}