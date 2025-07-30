

using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IInventoryServices
    {

        (bool, List<InventoryVM>?) GetAll();
        (bool, InventoryVM?) GetByCompany(int companyId);
        bool AddItem(int companyId, int gameId, int quantity, decimal price);
        bool UpdateItem(int inventoryItemId, int quantity, decimal price);
        bool DeleteInventoryItem(int companyId, int inventoryItemId);
    }
}
