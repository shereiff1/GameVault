using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
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

        public (bool, List<InventoryItemVM>?) GetAll()
        {
            var (success, items) = _inventoryItemRepo.GetAll();
            if (!success || items == null) return (false, null);

            return (true, _mapper.Map<List<InventoryItemVM>>(items));
        }

        public (bool, InventoryItemVM?) GetById(int inventoryItemId)
        {
            var (success, item) = _inventoryItemRepo.GetById(inventoryItemId);
            if (!success || item == null) return (false, null);

            return (true, _mapper.Map<InventoryItemVM>(item));
        }

        public (bool, List<InventoryItemVM>?) GetByGame(int gameId)
        {
            var (success, items) = _inventoryItemRepo.GetByGame(gameId);
            if (!success || items == null) return (false, null);

            return (true, _mapper.Map<List<InventoryItemVM>>(items));
        }

        public (bool, List<InventoryItemVM>?) GetByCompany(int companyid)
        {
            var (success, items) = _inventoryItemRepo.GetByCompany(companyid);
            if (!success || items == null) return (false, null);

            return (true, _mapper.Map<List<InventoryItemVM>>(items));
        }

        public bool Delete(int inventoryItemId)
        {
            return _inventoryItemRepo.Delete(inventoryItemId);
        }


    }
}
