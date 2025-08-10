using GameVault.DAL.Entites;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IGameRepo
    {
        Task<bool> AddAsync(Game game, decimal price, List<int>? categoryIds = null);
        Task<bool> DeleteAsync(int gameId);
        Task<(bool, List<Game>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, Game?)> GetByIdAsync(int gameId);
        Task<(bool, Game?, decimal)> GetByIdWithPriceAsync(int gameId);
        Task<List<int>> GetGameCategoryIdsAsync(int gameId);
        Task<(bool, List<Game>)> GetByCompanyAsync(int companyId);
        Task<bool> UpdateAsync(Game game, decimal price, List<int>? categoryIds = null);
        Task<(bool, List<GameDTO>?)> GetAllGameDetailsAsync();
        Task<(bool success, Game? game)> GetGameDetails(int id);

        // Legacy methods for backward compatibility
        Task<bool> AddAsync(Game game, decimal price);
        Task<bool> UpdateAsync(Game game, decimal price);
    }
}