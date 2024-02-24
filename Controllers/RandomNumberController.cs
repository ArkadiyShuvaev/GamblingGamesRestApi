using GamblingGamesRestApi.Models;
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
    public RandomNumberController()
    {

    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] BetRequest betRequest)
    {
        var user = User;
        var user2 = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(new { number = new Random().Next(1, 100) });
    }
}
