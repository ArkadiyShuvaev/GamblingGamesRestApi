namespace GamblingGamesRestApi.Repositories
{
    public interface IPointRepository
    {
        Task AddAsync(string email, int points);
        Task<int> GetAsync(string email);
    }
}