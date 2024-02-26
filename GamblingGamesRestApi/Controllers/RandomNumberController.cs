using GamblingGamesRestApi.Exceptions;
using GamblingGamesRestApi.Extensions;
using GamblingGamesRestApi.Models;
using GamblingGamesRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamblingGamesRestApi.Controllers;

/// <summary>
/// Provide members for the random number game.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[Produces("application/json")]
public class RandomNumberController : ControllerBase
{
    private readonly IBetService _betService;
    private readonly ILogger<RandomNumberController> _logger;

    public RandomNumberController(IBetService betService,
        ILogger<RandomNumberController> logger)
    {
        _betService = betService;
        _logger = logger;
    }

    /// <summary>
    /// Places a bet on a number.
    /// </summary>
    /// <response code = "201">The bet has been placed.</response>
    /// <response code = "400">The request is invalid.</response>
    /// <response code = "500">An error occurred while processing the request.</response>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] BetRequest betRequest)
    {
        try
        {
            var email = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("The request to place a bet: {BetRequest}, email: {Email}.", betRequest, email);

            var betReqult = await _betService.PlaceBetAsync(email, betRequest.Number, betRequest.Points);

            return StatusCode(StatusCodes.Status201Created, betReqult);
        }
        catch (GameValidationException ex)
        {
            _logger.LogWarning(ex, "An error occurred while placing a bet: {Error}.", ex.Message);
            return ValidationProblem(new ValidationProblemDetails(ex.ToValidationDictionary()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while placing a bet: {Error}.", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
        }
    }
}
