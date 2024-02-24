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

            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while placing a bet: {Error}.", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
        }
    }
}
