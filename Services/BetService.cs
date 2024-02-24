using GamblingGamesRestApi.Repositories;

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

    public async Task PlaceBetAsync(string email, int number, int points)
    {
        await _pointRepository.UpdateAsync(email, -points);

        _logger.LogInformation("The result of user '{Email}' bet placement: {Points}", email, points);
    }
}
