using System.ComponentModel.DataAnnotations;

namespace GamblingGamesRestApi.Models;

public class LoginRequestModel
{
    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    /// <example>P@ssw0rd!</example>
    [Required]
    public string Password { get; set; }
}