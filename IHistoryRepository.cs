public interface IHistoryRepository
{
    Task<IEnumerable<History>> GetAllAsync();
    Task<History> GetByIdAsync(int id);
    Task AddAsync(History history);
    Task UpdateAsync(History history);
    Task DeleteAsync(int id);
}