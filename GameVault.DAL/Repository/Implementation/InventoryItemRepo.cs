//using GameVault.DAL.Database;
//using GameVault.DAL.Entities;
//using GameVault.DAL.Repository.Abstraction;
//using Microsoft.EntityFrameworkCore;

//namespace GameVault.DAL.Repository.Implementation
//{
//    public class InventoryItemRepo : IInventoryItemRepo
//    {
//        private readonly ApplicationDbContext _context;

//        public InventoryItemRepo(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<bool> AddAsync(InventoryItem item)
//        {
//            try
//            {
//                await _context.inventoryItems.AddAsync(item);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return false;
//            }
//        }

//        public async Task<bool> DeleteAsync(int inventoryItemId)
//        {
//            try
//            {
//                var item = await _context.inventoryItems
//                    .FirstOrDefaultAsync(i => i.InventoryItemId == inventoryItemId);

//                if (item == null) return false;

//                _context.inventoryItems.Remove(item);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return false;
//            }
//        }

//        public async Task<(bool, List<InventoryItem>?)> GetAllAsync(bool includeDeleted = false)
//        {
//            try
//            {
//                var list = await _context.inventoryItems
//                    .Include(i => i.Game)
//                    .Include(i => i.Inventory)
//                        .ThenInclude(inv => inv.Company)
//                    .ToListAsync();

//                return (true, list);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return (false, null);
//            }
//        }

//        public async Task<(bool, InventoryItem?)> GetByIdAsync(int inventoryItemId)
//        {
//            try
//            {
//                var item = await _context.inventoryItems
//                    .Include(i => i.Game)
//                    .Include(i => i.Inventory)
//                    .FirstOrDefaultAsync(i => i.InventoryItemId == inventoryItemId);

//                return (item != null, item);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return (false, null);
//            }
//        }

//        public async Task<(bool, List<InventoryItem>)> GetByCompanyAsync(int companyId)
//        {
//            try
//            {
//                var list = await _context.inventoryItems
//                    .Where(i => i.CompanyId == companyId)
//                    .Include(i => i.Game)
//                    .ToListAsync();

//                return (true, list);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return (false, new List<InventoryItem>());
//            }
//        }

//        public async Task<(bool, List<InventoryItem>)> GetByGameAsync(int gameId)
//        {
//            try
//            {
//                var list = await _context.inventoryItems
//                    .Where(i => i.GameId == gameId)
//                    .Include(i => i.Inventory)
//                        .ThenInclude(inv => inv.Company)
//                    .ToListAsync();

//                return (true, list);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return (false, new List<InventoryItem>());
//            }
//        }

//        public async Task<bool> UpdateAsync(InventoryItem item)
//        {
//            try
//            {
//                _context.inventoryItems.Update(item);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return false;
//            }
//        }
//    }
//}
