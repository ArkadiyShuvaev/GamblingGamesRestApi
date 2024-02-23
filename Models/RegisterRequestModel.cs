using System.ComponentModel.DataAnnotations;

namespace GamblingGamesRestApi.Models;

public class RegisterRequestModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}