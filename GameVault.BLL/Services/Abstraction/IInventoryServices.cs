using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IInventoryServices
    {
        Task<(bool, List<InventoryVM>?)> GetAllAsync();
        Task<(bool, InventoryVM?)> GetByCompanyAsync(int companyId);
        Task<bool> AddItemAsync(int companyId, int gameId, int quantity, decimal price);
        Task<bool> UpdateItemAsync(int inventoryItemId, int quantity, decimal price);
        Task<bool> DeleteInventoryItemAsync(int companyId, int inventoryItemId);
    }
}
