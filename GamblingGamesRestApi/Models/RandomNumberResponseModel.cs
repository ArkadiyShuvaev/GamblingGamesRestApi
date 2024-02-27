namespace GamblingGamesRestApi.Models;

/// <summary>
/// Represents the result of the bet create request.
/// </summary>
public class RandomNumberResponseModel
{
    public string Email { get; set; }

    public string Status { get; set; }

    public string Points { get; set; }

    public int PointsTotal { get; set; }
}
