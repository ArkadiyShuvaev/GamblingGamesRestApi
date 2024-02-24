using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamblingGamesRestApi.Entities;

[Table("Point")]
[Index(nameof(Email), IsUnique = true)]
public class PointEntity
{
    [Key]
    [MaxLength(255)]
    [Required]
    public string Email { get; set; }


    /// <summary>
    /// Gets or sets the number of points the player has.
    /// The player has a starting account of 10,000 points and can wager on a prediction that they will either win or lose.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public int Value { get; set; }
}
