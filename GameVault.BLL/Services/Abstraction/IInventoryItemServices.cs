

using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IInventoryItemServices
    {
        (bool, List<InventoryItemVM>?) GetAll();
        (bool, InventoryItemVM?) GetById(int inventoryItemId);
        (bool, List<InventoryItemVM>?) GetByGame(int gameId);
        (bool, List<InventoryItemVM>?) GetByCompany(int companyid);
        bool Delete(int inventoryItemId);
    }
}
