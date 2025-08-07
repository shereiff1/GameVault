using GameVault.DAL.Entites;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IGameRepo
    {
        Task<bool> AddAsync(Game game, decimal price);
        Task<bool> DeleteAsync(int gameId);
        Task<(bool, List<Game>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, Game?)> GetByIdAsync(int gameId);
        Task<(bool success, Game? game, decimal price)> GetByIdWithPriceAsync(int gameId);
        Task<(bool, List<Game>)> GetByCompanyAsync(int companyId);
        Task<bool> UpdateAsync(Game game, decimal price);
        Task<(bool, List<GameDTO>?)> GetAllGameDetailsAsync();
        Task<(bool success, Game? game)> GetGameDetails(int id);
    }
}
