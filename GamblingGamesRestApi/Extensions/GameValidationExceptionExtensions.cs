using GamblingGamesRestApi.Exceptions;

namespace GamblingGamesRestApi.Extensions;

public static class GameValidationExceptionExtensions
{
    public static Dictionary<string, string[]> ToValidationDictionary(this GameValidationException validationException)
    {
        var validationDictionary = new Dictionary<string, string[]>
        {
            { validationException.ErrorCode, new string[] { validationException.Message } }
        };

        return validationDictionary;
    }
}
