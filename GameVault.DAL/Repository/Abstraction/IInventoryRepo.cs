using System.Collections.Generic;
using System.Threading.Tasks;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IInventoryRepo
    {
        Task<bool> AddItemAsync(int companyId, InventoryItem newItem);

        Task<(bool Success, List<Inventory>? Inventories)> GetAllAsync(bool includeDeleted = false);

        Task<(bool Success, Inventory? Inventory)> GetByIdAsync(int inventoryId);

        Task<(bool Success, Inventory? Inventory)> GetByCompanyAsync(int companyId);

        Task<bool> UpdateAsync(Inventory inventory);

        Task<bool> DeleteAsync(int companyId, int inventoryItemId);
    }
}
