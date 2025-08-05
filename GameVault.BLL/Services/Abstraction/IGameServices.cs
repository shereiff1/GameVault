using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Game;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IGameServices
    {
        Task<bool> AddAsync(GameVM gameVM);
        Task<(bool, List<GameVM>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, EditGame?)> GetByIdAsync(int gameId);
        Task<bool> UpdateAsync(EditGame editGame);
        Task<bool> DeleteAsync(int gameId);
        Task<(bool, List<GameVM>)> GetByCompanyAsync(int companyId);
        Task<(bool, List<GameDetails>?)> GetAllGameDetailsAsync();
    }
}
