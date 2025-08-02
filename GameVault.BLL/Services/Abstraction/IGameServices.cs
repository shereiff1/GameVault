using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IGameServices
    {
        Task<bool> AddAsync(int companyId, GameVM gameVM);
        Task<(bool, List<GameVM>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, GameVM?)> GetByIdAsync(int gameId);
        Task<bool> UpdateAsync(GameVM gameVM);
        Task<bool> DeleteAsync(int gameId);
        Task<(bool, List<GameVM>)> GetByCompanyAsync(int companyId);
    }
}
