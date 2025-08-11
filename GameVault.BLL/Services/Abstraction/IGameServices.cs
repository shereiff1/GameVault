using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Game;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IGameServices
    {
        Task<bool> AddAsync(ModelVM.GameVM gameVM);
        Task<(bool, List<ModelVM.GameVM>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, EditGame?)> GetByIdAsync(int gameId);
        Task<bool> UpdateAsync(EditGame editGame);
        Task<bool> DeleteAsync(int gameId);
        Task<(bool, List<ModelVM.GameVM>)> GetByCompanyAsync(int companyId);
        Task<(bool, List<GameDetails>?)> GetAllGameDetailsAsync();
        Task<(bool success, GameDetails)> GetGameDetails(int id);
    }
}
