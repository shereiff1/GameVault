using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.BLL.Services.Implementation
{
    public class InventoryItemServices : IInventoryItemServices
    {
        private readonly IInventoryItemRepo _inventoryItemRepo;
        private readonly IMapper _mapper;

        public InventoryItemServices(IInventoryItemRepo inventoryItemRepo, IMapper mapper)
        {
            _inventoryItemRepo = inventoryItemRepo;
            _mapper = mapper;
        }

        public async Task<(bool, List<InventoryItemVM>?)> GetAllAsync()
        {
            var (success, items) = await _inventoryItemRepo.GetAllAsync();
            if (!success || items == null) return (false, null);

            return (true, _mapper.Map<List<InventoryItemVM>>(items));
        }

        public async Task<(bool, InventoryItemVM?)> GetByIdAsync(int inventoryItemId)
        {
            var (success, item) = await _inventoryItemRepo.GetByIdAsync(inventoryItemId);
            if (!success || item == null) return (false, null);

            return (true, _mapper.Map<InventoryItemVM>(item));
        }

        public async Task<(bool, List<InventoryItemVM>?)> GetByGameAsync(int gameId)
        {
            var (success, items) = await _inventoryItemRepo.GetByGameAsync(gameId);
            if (!success || items == null) return (false, null);

            return (true, _mapper.Map<List<InventoryItemVM>>(items));
        }

        public async Task<(bool, List<InventoryItemVM>?)> GetByCompanyAsync(int companyId)
        {
            var (success, items) = await _inventoryItemRepo.GetByCompanyAsync(companyId);
            if (!success || items == null) return (false, null);

            return (true, _mapper.Map<List<InventoryItemVM>>(items));
        }

        public async Task<bool> DeleteAsync(int inventoryItemId)
        {
            return await _inventoryItemRepo.DeleteAsync(inventoryItemId);
        }
    }
}
