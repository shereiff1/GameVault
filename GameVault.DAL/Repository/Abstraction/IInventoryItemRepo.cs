using System.Collections.Generic;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IInventoryItemRepo
    {
        bool Add(InventoryItem item);
        (bool, List<InventoryItem>?) GetAll(bool includeDeleted = false);
        (bool, InventoryItem?) GetById(int inventoryItemId);
        bool Update(InventoryItem item);
        bool Delete(int inventoryItemId);
        (bool, List<InventoryItem>) GetByCompany(int companyid);
        (bool, List<InventoryItem>) GetByGame(int gameId);
    }
}
