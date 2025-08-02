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

        public async Task<bool> AddItemAsync(int companyId, InventoryItem newItem)
        {
            try
            {
                var inv = await _context.inventories
                                        .Include(i => i.Items)
                                        .FirstOrDefaultAsync(i => i.CompanyId == companyId);

                if (inv == null)
                    return false;

                inv.Items.Add(newItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<(bool Success, List<Inventory>? Inventories)> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var list = await _context.inventories
                                         .Include(inv => inv.Company)
                                         .Include(inv => inv.Items)
                                            .ThenInclude(item => item.Game)
                                         .ToListAsync();
                return (true, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<(bool Success, Inventory? Inventory)> GetByIdAsync(int inventoryId)
        {
            try
            {
                var inv = await _context.inventories
                                        .Include(i => i.Company)
                                        .Include(i => i.Items)
                                            .ThenInclude(item => item.Game)
                                        .FirstOrDefaultAsync(i => i.CompanyId == inventoryId);

                return (inv != null, inv);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<(bool Success, Inventory? Inventory)> GetByCompanyAsync(int companyId)
        {
            try
            {
                var inv = await _context.inventories
                                        .Include(i => i.Company)
                                        .Include(i => i.Items)
                                            .ThenInclude(item => item.Game)
                                        .FirstOrDefaultAsync(i => i.CompanyId == companyId);

                return (inv != null, inv);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<bool> UpdateAsync(Inventory inventory)
        {
            try
            {
                _context.inventories.Update(inventory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int companyId, int inventoryItemId)
        {
            try
            {
                var itemToRemove = await _context.inventoryItems
                    .Include(i => i.Inventory)
                    .FirstOrDefaultAsync(i => i.InventoryItemId == inventoryItemId &&
                                              i.Inventory.CompanyId == companyId);

                if (itemToRemove == null)
                    return false;

                _context.inventoryItems.Remove(itemToRemove);
                await _context.SaveChangesAsync();
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
