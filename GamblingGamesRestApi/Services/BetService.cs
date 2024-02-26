using GamblingGamesRestApi.Exceptions;
using GamblingGamesRestApi.Models;
using GamblingGamesRestApi.Repositories;

namespace GamblingGamesRestApi.Services;

/// <summary>
/// Represents members to place a bet.
/// </summary>
public class BetService : IBetService
{
    private readonly IPointRepository _pointRepository;

    public BetService(IPointRepository pointRepository)
    {
        _pointRepository = pointRepository;
    }

    public async Task<BetProcessingResult> PlaceBetAsync(string email, int number, int points)
    {
        var currentPoints = await _pointRepository.GetAsync(email);

        if (currentPoints - points < 0)
        {
            throw new GameValidationException($"The number of the existing points is insufficient to start the game. Points on the balance: {currentPoints}", "Insufficient points");
        }

        var randomNumber = new Random().Next(0, 10);
        if (number == randomNumber)
        {
            var wonPoints = points * 9;

            await _pointRepository.UpdateAsync(email, currentPoints + wonPoints);
            return new BetProcessingResult
            {
                Email = email,
                Status = "won",
                Points = $"+{wonPoints}",
                PointsTotal = currentPoints + wonPoints
            };
        }

        await _pointRepository.UpdateAsync(email, -points);
        return new BetProcessingResult
        {
            Email = email,
            Status = "lost",
            Points = $"-{points}",
            PointsTotal = currentPoints - points
        };
    }
}
