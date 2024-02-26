using GamblingGamesRestApi.Models;

namespace GamblingGamesRestApi.Services;

public interface IBetService
{
    /// <summary>
    /// Creates a bet for the specified user.
    /// </summary>
    Task<BetCreateResponseModel> PlaceBetAsync(string email, int number, int points);
}