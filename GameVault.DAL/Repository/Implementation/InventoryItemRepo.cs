using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace GameVault.DAL.Repository.Implementation
{
    public class InventoryItemRepo : IInventoryItemRepo
    {
        private readonly ApplicationDbContext _context;

        public InventoryItemRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(InventoryItem item)
        {
            try
            {
                _context.inventoryItems.Add(item);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Delete(int inventoryItemId)
        {
            try
            {
                var item = _context.inventoryItems.FirstOrDefault(i => i.InventoryItemId == inventoryItemId);
                if (item == null) return false;

                _context.inventoryItems.Remove(item);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public (bool, List<InventoryItem>?) GetAll(bool includeDeleted = false)
        {
            try
            {
                var list = _context.inventoryItems
                                   .Include(i => i.Game)
                                   .Include(i => i.Inventory).ThenInclude(inv => inv.Company)
                                   .ToList();
                return (true, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public (bool, InventoryItem?) GetById(int inventoryItemId)
        {
            try
            {
                var item = _context.inventoryItems
                                   .Include(i => i.Game)
                                   .Include(i => i.Inventory)
                                   .FirstOrDefault(i => i.InventoryItemId == inventoryItemId);
                return (item != null, item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public (bool, List<InventoryItem>) GetByCompany(int companyid)
        {
            try
            {
                var list = _context.inventoryItems
                                   .Where(i => i.CompanyId == companyid)
                                   .Include(i => i.Game)
                                   .ToList();
                return (true, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new List<InventoryItem>());
            }
        }

        public (bool, List<InventoryItem>) GetByGame(int gameId)
        {
            try
            {
                var list = _context.inventoryItems
                                   .Where(i => i.GameId == gameId)
                                   .Include(i => i.Inventory).ThenInclude(inv => inv.Company)
                                   .ToList();
                return (true, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new List<InventoryItem>());
            }
        }

        public bool Update(InventoryItem item)
        {
            try
            {
                _context.inventoryItems.Update(item);
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
