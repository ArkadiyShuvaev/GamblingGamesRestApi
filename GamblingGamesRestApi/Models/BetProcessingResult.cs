namespace GamblingGamesRestApi.Models;

/// <summary>
/// Represents the result of a bet processing.
/// </summary>
public class BetProcessingResult
{
    public string Email { get; set; }

    public string Status { get; set; }

    public string Points { get; set; }

    public int PointsTotal { get; set; }
}
