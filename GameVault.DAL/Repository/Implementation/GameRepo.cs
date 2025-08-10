using GameVault.DAL.Database;
using GameVault.DAL.Entites;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace GameVault.DAL.Repository.Implementation
{
    public class GameRepo : IGameRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IInventoryItemRepo _inventoryItemRepo;

        public GameRepo(ApplicationDbContext context, IInventoryItemRepo inventoryItemRepo)
        {
            _context = context;
            _inventoryItemRepo = inventoryItemRepo;
        }

        public async Task<bool> AddAsync(Game game, decimal price, List<int>? categoryIds = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Find the company
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == game.CompanyId);
                if (company == null)
                    return false;

                // Add the Game
                await _context.games.AddAsync(game);
                await _context.SaveChangesAsync();

                // Add categories if provided
                if (categoryIds != null && categoryIds.Any())
                {
                    var categories = await _context.Categories
                        .Where(c => categoryIds.Contains(c.Category_Id) && !c.IsDeleted)
                        .ToListAsync();

                    game.Categories.AddRange(categories);
                    await _context.SaveChangesAsync();
                }

                var inventoryItem = new InventoryItem(company, game.GameId, price);
                await _context.inventoryItems.AddAsync(inventoryItem);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int gameId)
        {
            try
            {
                var game = await _context.games.FirstOrDefaultAsync(g => g.GameId == gameId);
                if (game == null) return false;

                game.MarkAsDeleted();
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<(bool, List<Game>?)> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var result = includeDeleted
                    ? await _context.games.Include(g => g.Categories).ToListAsync()
                    : await _context.games.Include(g => g.Categories).Where(g => !g.IsDeleted).ToListAsync();

                return (true, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<(bool, Game?)> GetByIdAsync(int gameId)
        {
            try
            {
                var game = await _context.games
                    .Include(g => g.Categories)
                    .FirstOrDefaultAsync(g => g.GameId == gameId && !g.IsDeleted);
                return (game != null, game);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<(bool, Game?, decimal)> GetByIdWithPriceAsync(int gameId)
        {
            try
            {
                var game = await _context.games
                    .Include(g => g.Categories)
                    .FirstOrDefaultAsync(g => g.GameId == gameId && !g.IsDeleted);
                if (game == null)
                    return (false, null, 0);

                var inventoryItem = await _context.inventoryItems
                    .FirstOrDefaultAsync(i => i.GameId == gameId);

                decimal price = inventoryItem?.Price ?? 0;
                return (true, game, price);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null, 0);
            }
        }

        public async Task<List<int>> GetGameCategoryIdsAsync(int gameId)
        {
            try
            {
                var game = await _context.games
                    .Include(g => g.Categories)
                    .FirstOrDefaultAsync(g => g.GameId == gameId && !g.IsDeleted);

                return game?.Categories?.Where(c => !c.IsDeleted).Select(c => c.Category_Id).ToList() ?? new List<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<int>();
            }
        }

        public async Task<(bool, List<Game>)> GetByCompanyAsync(int companyId)
        {
            try
            {
                var games = await _context.games
                    .Include(g => g.Categories)
                    .Where(g => g.CompanyId == companyId && !g.IsDeleted)
                    .ToListAsync();

                return (true, games);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new List<Game>());
            }
        }

        public async Task<bool> UpdateAsync(Game game, decimal price, List<int>? categoryIds = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingGame = await _context.games
                    .Include(g => g.Categories)
                    .FirstOrDefaultAsync(g => g.GameId == game.GameId && !g.IsDeleted);
                if (existingGame == null)
                    return false;

                existingGame.Update(game.Title, game.Description);
                existingGame.UpdateCompany(game.CompanyId);
                existingGame.UpdatePhoto(game.ImagePath);

                // Update categories
                if (categoryIds != null)
                {
                    // Clear existing categories
                    existingGame.Categories.Clear();

                    // Add new categories
                    var newCategories = await _context.Categories
                        .Where(c => categoryIds.Contains(c.Category_Id) && !c.IsDeleted)
                        .ToListAsync();

                    existingGame.Categories.AddRange(newCategories);
                }

                var inventoryItem = await _context.inventoryItems
                    .FirstOrDefaultAsync(i => i.GameId == game.GameId);

                if (inventoryItem != null)
                    inventoryItem.UpdatePrice(price);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<(bool, List<GameDTO>?)> GetAllGameDetailsAsync()
        {
            try
            {
                var gameDetails = await _context.games
                    .Where(g => !g.IsDeleted)
                    .Include(g => g.Company)
                    .Include(g => g.Reviews.Where(r => !r.IsDeleted))
                    .Include(g => g.Categories.Where(c => !c.IsDeleted))
                    .Select(g => new GameDTO
                    {
                        GameId = g.GameId,
                        Title = g.Title,
                        Description = g.Description,
                        CompanyName = g.Company.CompanyName,

                        Reviews = g.Reviews
                            .Where(r => !r.IsDeleted)
                            .Select(r => new Review(
                                r.Player_Id,
                                r.Comment,
                                r.Rating,
                                r.Game_Id,
                                r.CreatedBy
                            ))
                            .ToList(),

                        Categories = g.Categories
                            .Where(c => !c.IsDeleted)
                            .Select(c => new Category(
                                c.Category_Id,
                                c.Category_Name,
                                c.Description,
                                c.CreatedBy
                            ))
                            .ToList(),

                        Rating = g.Reviews
                            .Where(r => !r.IsDeleted)
                            .Any()
                                ? g.Reviews.Where(r => !r.IsDeleted).Average(r => r.Rating)
                                : 0,

                        Price = _context.inventoryItems
                            .Where(i => i.GameId == g.GameId)
                            .Select(i => i.Price)
                            .FirstOrDefault(),

                        ImagePath = g.ImagePath
                    })
                    .ToListAsync();

                return (true, gameDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting game details: {ex.Message}");
                return (false, null);
            }
        }


        public async Task<(bool success, Game? game)> GetGameDetails(int id)
        {
            try
            {
                var game = await _context.games
                    .Include(g => g.Company)
                    .Include(g => g.Reviews.Where(r => !r.IsDeleted))
                    .Include(g => g.Categories.Where(c => !c.IsDeleted))
                    .FirstOrDefaultAsync(g => g.GameId == id && !g.IsDeleted);

                return (game != null, game);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching game details: {ex.Message}");
                return (false, null);
            }
        }

        // Legacy methods for backward compatibility (if needed by existing code)
        public async Task<bool> AddAsync(Game game, decimal price)
        {
            return await AddAsync(game, price, null);
        }

        public async Task<bool> UpdateAsync(Game game, decimal price)
        {
            return await UpdateAsync(game, price, null);
        }
    }
}