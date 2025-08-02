using GameVault.DAL.Entites;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IGameRepo
    {
        Task<bool> AddAsync(Game game, int companyId);
        Task<(bool, List<Game>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, Game?)> GetByIdAsync(int gameId);
        Task<bool> UpdateAsync(Game game);
        Task<bool> DeleteAsync(int gameId);
        Task<(bool, List<Game>)> GetByCompanyAsync(int companyId);
    }
}
