using GamblingGamesRestApi.Exceptions;
using GamblingGamesRestApi.Models;
using GamblingGamesRestApi.Repositories;
using System.ComponentModel.DataAnnotations;

namespace GamblingGamesRestApi.Services;

/// <summary>
/// Represents members to place a bet.
/// </summary>
public class BetService : IBetService
{
    private readonly IPointRepository _pointRepository;
    private readonly ILogger<BetService> _logger;

    public BetService(IPointRepository pointRepository,
               ILogger<BetService> logger)
    {
        _pointRepository = pointRepository;
        _logger = logger;
    }

    public async Task<BetProcessingResult> PlaceBetAsync(string email, int number, int points)
    {
        var currentPoints = await _pointRepository.GetAsync(email);

        if (currentPoints < points)
        {
            throw new GameValidationException($"Insufficient points. The current points: {currentPoints}", "Insufficient points");
        }

        await _pointRepository.UpdateAsync(email, -points);

        _logger.LogInformation("The result of user '{Email}' bet placement: {Points}", email, points);
    }
}
