using GamblingGamesRestApi.Models;

namespace GamblingGamesRestApi.Services;

public interface IRandomNumberService
{
    /// <summary>
    /// Creates a bet against the number.
    /// </summary>
    Task<RandomNumberResponseModel> CreateAsync(string email, int number, int points);
}