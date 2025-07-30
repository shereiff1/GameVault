public class HistoryRepository : IHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public HistoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<History>> GetAllAsync()
    {
        return await _context.Histories.ToListAsync();
    }

    public async Task<History> GetByIdAsync(int id)
    {
        return await _context.Histories.FindAsync(id);
    }

    public async Task AddAsync(History history)
    {
        await _context.Histories.AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(History history)
    {
        _context.Histories.Update(history);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var history = await _context.Histories.FindAsync(id);
        if (history != null)
        {
            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();
        }
    }
}