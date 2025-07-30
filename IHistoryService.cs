public interface IHistoryService
{
    Task<IEnumerable<History>> GetAllHistoriesAsync();
    Task<History> GetHistoryByIdAsync(int id);
    Task CreateHistoryAsync(History history);
    Task UpdateHistoryAsync(History history);
    Task DeleteHistoryAsync(int id);
}