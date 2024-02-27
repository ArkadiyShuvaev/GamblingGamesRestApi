using System.ComponentModel.DataAnnotations;

namespace GamblingGamesRestApi.Models;

/// <summary>
/// Represents a request to place a bet.
/// </summary>
public class RandomNumberRequestModel
{
    /// <summary>
    /// Gets or sets the number to predict.
    /// </summary>
    [Required]
    [Range(0, 9)]
    public int Number { get; set; }

    /// <summary>
    /// The number of points is used to predict the number.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Points { get; set; }

    public override string ToString()
    {
        return $"{nameof(Number)}={Number.ToString()}, {nameof(Points)}={Points.ToString()}";
    }
}
