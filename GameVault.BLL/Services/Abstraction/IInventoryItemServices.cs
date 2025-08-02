using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IInventoryItemServices
    {
        Task<(bool, List<InventoryItemVM>?)> GetAllAsync();
        Task<(bool, InventoryItemVM?)> GetByIdAsync(int inventoryItemId);
        Task<(bool, List<InventoryItemVM>?)> GetByGameAsync(int gameId);
        Task<(bool, List<InventoryItemVM>?)> GetByCompanyAsync(int companyId);
        Task<bool> DeleteAsync(int inventoryItemId);
    }
}
