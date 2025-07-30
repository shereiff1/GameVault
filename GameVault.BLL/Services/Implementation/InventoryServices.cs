

using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.BLL.Services.Implementation
{
    public class InventoryServices : IInventoryServices
    {
        private readonly IInventoryRepo _inventoryRepo;
        private readonly IInventoryItemRepo _inventoryItemRepo;
        private readonly IMapper _mapper;

        public InventoryServices(IInventoryRepo inventoryRepo, IInventoryItemRepo inventoryItemRepo, IMapper mapper)
        {
            _inventoryRepo = inventoryRepo;
            _inventoryItemRepo = inventoryItemRepo;
            _mapper = mapper;
        }

        public (bool, List<InventoryVM>?) GetAll()
        {
            var (success, inventories) = _inventoryRepo.GetAll();
            if (!success || inventories == null) return (false, null);

            var vmList = _mapper.Map<List<InventoryVM>>(inventories);
            return (true, vmList);
        }

        public (bool, InventoryVM?) GetByCompany(int companyId)
        {
            var (success, inventory) = _inventoryRepo.GetByCompany(companyId);
            if (!success || inventory == null) return (false, null);

            var vm = _mapper.Map<InventoryVM>(inventory);
            return (true, vm);
        }

        public bool AddItem(int companyId, int gameId, int quantity, decimal price)
        {
            var (success, inventory) = _inventoryRepo.GetByCompany(companyId);
            if (!success || inventory == null) return false;

            var item = new InventoryItem(inventory, gameId, quantity, price);
            return _inventoryRepo.AddItem(companyId, item);
        }

        public bool UpdateItem(int inventoryItemId, int quantity, decimal price)
        {
            var (success, item) = _inventoryItemRepo.GetById(inventoryItemId);
            if (!success || item == null) return false;

            item.SetQuantity(quantity);
            item.SetPrice(price);
            return _inventoryItemRepo.Update(item);
        }

        public bool DeleteInventoryItem(int companyId,int inventoryItemId)
        {
            return _inventoryRepo.Delete(companyId, inventoryItemId);
        }
    }
}
