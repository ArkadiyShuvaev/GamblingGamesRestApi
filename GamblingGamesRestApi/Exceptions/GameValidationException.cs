namespace GamblingGamesRestApi.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a game validation fails.
/// </summary>
public class GameValidationException : Exception
{
    /// <summary>
    /// Gets the error code of the validation error.
    /// </summary>
    public string ErrorCode { get; }

    public GameValidationException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
