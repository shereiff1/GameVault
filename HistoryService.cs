public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;

    public HistoryService(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public Task<IEnumerable<History>> GetAllHistoriesAsync() => _historyRepository.GetAllAsync();
    public Task<History> GetHistoryByIdAsync(int id) => _historyRepository.GetByIdAsync(id);
    public Task CreateHistoryAsync(History history) => _historyRepository.AddAsync(history);
    public Task UpdateHistoryAsync(History history) => _historyRepository.UpdateAsync(history);
    public Task DeleteHistoryAsync(int id) => _historyRepository.DeleteAsync(id);
}