namespace GamblingGamesRestApi.Repositories;

public interface IPointRepository
{
    /// <summary>
    /// Updates the points of the user with the given email.
    /// </summary>
    Task UpdateAsync(string email, int points);

    /// <summary>
    /// Returns the number of points the user with the given email has.
    /// </summary>
    Task<int> GetAsync(string email);
}