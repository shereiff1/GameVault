using System.Collections.Generic;
using System.Threading.Tasks;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IInventoryItemRepo
    {
        Task<bool> AddAsync(InventoryItem item);
        Task<(bool, List<InventoryItem>?)> GetAllAsync(bool includeDeleted = false);
        Task<(bool, InventoryItem?)> GetByIdAsync(int inventoryItemId);
        Task<bool> UpdateAsync(InventoryItem item);
        Task<bool> DeleteAsync(int inventoryItemId);
        Task<(bool, List<InventoryItem>)> GetByCompanyAsync(int companyId);
        Task<(bool, List<InventoryItem>)> GetByGameAsync(int gameId);
    }
}
