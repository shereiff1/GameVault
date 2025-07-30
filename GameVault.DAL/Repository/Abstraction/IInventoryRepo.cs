using System.Collections.Generic;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IInventoryRepo
    {

        bool AddItem(int companyId, InventoryItem newItem);
        (bool Success, List<Inventory>? Inventories) GetAll(bool includeDeleted = false);

        (bool Success, Inventory? Inventory) GetById(int inventoryId);

        (bool Success, Inventory? Inventory) GetByCompany(int companyId);

        bool Update(Inventory inventory);

        bool Delete(int inventoryId, int inventoryItemId);
    }
}
