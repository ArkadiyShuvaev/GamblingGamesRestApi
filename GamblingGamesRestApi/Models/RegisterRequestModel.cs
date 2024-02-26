using System.ComponentModel.DataAnnotations;

namespace GamblingGamesRestApi.Models;

public class RegisterRequestModel
{
    /// <summary>
    /// Gets or sets the email of the new user.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the new user.
    /// </summary>
    /// <example>P@ssw0rd!</example>
    [Required]
    public string Password { get; set; }
}