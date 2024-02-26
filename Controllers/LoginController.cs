using GamblingGamesRestApi.Exceptions;
using GamblingGamesRestApi.Models;
using GamblingGamesRestApi.Services;
using GamblingGamesRestApi.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamblingGamesRestApi.Controllers;

/// <summary>
/// Provides members to create a new account and login.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ServiceSettings _settings;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IUserService userService,
        ServiceSettings settings,
        ILogger<LoginController> logger)
    {
        _userService = userService;
        _settings = settings;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new user and assigns a staring point set.
    /// </summary>
    [HttpPost]
    [Route("Register")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        try
        {
            var result = await _userService.CreateAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created, model.Email);
            }

            return ValidationProblem(new ValidationProblemDetails(ToValidationDictionary(result.Errors)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Someting went wrong: {Error}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }
    }

    /// <summary>
    /// Creates a new user and assigns a staring point set.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(LoginRequestModel model)
    {
        try
        {
            var isPasswordCorrect = await _userService.CheckPasswordAsync(model.Email, model.Password);
            _logger.LogInformation("The login result for the email '{Email}': {Result}", model.Email, isPasswordCorrect);

            if (isPasswordCorrect)
            {
                var user = await _userService.GetAsync(model.Email);
                var token = GenerateJwtToken(user);

                return Ok(new LoginResponseModel { Token = token });
            }

            return Unauthorized("The username or login is incorrect.");
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "The user '{Email}' cannot be found.", model.Email);
            return Unauthorized("The username or login is incorrect.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Someting went wrong: {Error}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }
    }

    private Dictionary<string, string[]> ToValidationDictionary(IEnumerable<IdentityError> errors)
    {
        var validationDictionary = new Dictionary<string, string[]>();
        foreach (var error in errors)
        {
            validationDictionary.Add(error.Code, new string[] { error.Description });
        }

        return validationDictionary;
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddHours(1);

        var token = new JwtSecurityToken(
            "GamblingGamesCompany",
            "GamblingGamesCompany",
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
