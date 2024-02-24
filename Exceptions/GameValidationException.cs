namespace GamblingGamesRestApi.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a game validation fails.
/// </summary>
public class GameValidationException : ArgumentException
{
    public GameValidationException(string? message, string? paramName) : base(message, paramName)
    {
    }
}
