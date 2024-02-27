using GamblingGamesRestApi.Exceptions;
using GamblingGamesRestApi.Models;
using GamblingGamesRestApi.Repositories;

namespace GamblingGamesRestApi.Services;

/// <summary>
/// Represents members to place a bet.
/// </summary>
public class RandomNumberService : IRandomNumberService
{
    private readonly IPointRepository _pointRepository;

    public RandomNumberService(IPointRepository pointRepository)
    {
        _pointRepository = pointRepository;
    }

    public async Task<RandomNumberResponseModel> CreateAsync(string email, int number, int points)
    {
        var currentPoints = await _pointRepository.GetAsync(email);

        if (currentPoints - points < 0)
        {
            throw new GameValidationException("The number of points is insufficient to start the game. " +
                $"Points on the balance: {currentPoints}", "Insufficient points");
        }

        var randomNumber = new Random().Next(0, 10);
        if (number == randomNumber)
        {
            var wonPoints = points * 9;

            await _pointRepository.UpdateAsync(email, currentPoints + wonPoints);
            return new RandomNumberResponseModel
            {
                Email = email,
                Status = "won",
                Points = $"+{wonPoints}",
                PointsTotal = currentPoints + wonPoints
            };
        }

        await _pointRepository.UpdateAsync(email, -points);
        return new RandomNumberResponseModel
        {
            Email = email,
            Status = "lost",
            Points = $"-{points}",
            PointsTotal = currentPoints - points
        };
    }
}
