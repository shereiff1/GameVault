using Microsoft.EntityFrameworkCore;
using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.DAL.Repository.Implementation
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddItem(int companyId, InventoryItem newItem)
        {
            try
            {
                var inv = _context.inventories
                                  .Include(i => i.Items)
                                  .FirstOrDefault(i => i.CompanyId == companyId);

                if (inv == null)
                    return false;

                inv.Items.Add(newItem);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public (bool Success, List<Inventory>? Inventories) GetAll(bool includeDeleted = false)
        {
            try
            {
                var list = _context.inventories
                                   .Include(inv => inv.Company)
                                   .Include(inv => inv.Items)
                                     .ThenInclude(item => item.Game)
                                   .ToList();
                return (true, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public (bool Success, Inventory? Inventory) GetById(int inventoryId)
        {
            try
            {
                var inv = _context.inventories
                                  .Include(i => i.Company)
                                  .Include(i => i.Items)
                                    .ThenInclude(item => item.Game)
                                  .FirstOrDefault(i => i.CompanyId == inventoryId);
                return (inv != null, inv);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public (bool Success, Inventory? Inventory) GetByCompany(int companyId)
        {
            try
            {
                var inv = _context.inventories
                                  .Include(i => i.Company)
                                  .Include(i => i.Items)
                                    .ThenInclude(item => item.Game)
                                  .FirstOrDefault(i => i.CompanyId == companyId);
                return (inv != null, inv);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public bool Update(Inventory inventory)
        {
            try
            {
                _context.inventories.Update(inventory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Delete(int companyId, int inventoryItemId)
        {
            try
            {
                var itemToRemove = _context.inventoryItems
                    .Include(i => i.Inventory)
                    .FirstOrDefault(i => i.InventoryItemId == inventoryItemId
                                     && i.Inventory.CompanyId == companyId);

                if (itemToRemove == null)
                    return false;

                _context.inventoryItems.Remove(itemToRemove);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
