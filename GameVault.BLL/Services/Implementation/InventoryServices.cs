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

        public async Task<(bool, List<InventoryVM>?)> GetAllAsync()
        {
            var (success, inventories) = await _inventoryRepo.GetAllAsync();
            if (!success || inventories == null) return (false, null);

            var vmList = _mapper.Map<List<InventoryVM>>(inventories);
            return (true, vmList);
        }

        public async Task<(bool, InventoryVM?)> GetByCompanyAsync(int companyId)
        {
            var (success, inventory) = await _inventoryRepo.GetByCompanyAsync(companyId);
            if (!success || inventory == null) return (false, null);

            var vm = _mapper.Map<InventoryVM>(inventory);
            return (true, vm);
        }

        public async Task<bool> AddItemAsync(int companyId, int gameId, int quantity, decimal price)
        {
            var (success, inventory) = await _inventoryRepo.GetByCompanyAsync(companyId);
            if (!success || inventory == null) return false;

            var item = new InventoryItem(inventory, gameId, quantity, price);
            return await _inventoryRepo.AddItemAsync(companyId, item);
        }

        public async Task<bool> UpdateItemAsync(int inventoryItemId, int quantity, decimal price)
        {
            var (success, item) = await _inventoryItemRepo.GetByIdAsync(inventoryItemId);
            if (!success || item == null) return false;

            item.SetQuantity(quantity);
            item.SetPrice(price);
            return await _inventoryItemRepo.UpdateAsync(item);
        }

        public async Task<bool> DeleteInventoryItemAsync(int companyId, int inventoryItemId)
        {
            return await _inventoryRepo.DeleteAsync(companyId, inventoryItemId);
        }
    }
}
