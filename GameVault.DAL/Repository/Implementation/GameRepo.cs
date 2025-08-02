    using GameVault.DAL.Database;
    using GameVault.DAL.Entites;
    using GameVault.DAL.Repository.Abstraction;
    using Microsoft.EntityFrameworkCore;

    namespace GameVault.DAL.Repository.Implementation
    {
        public class GameRepo : IGameRepo
        {
            private readonly ApplicationDbContext _context;

            public GameRepo(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> AddAsync(Game game, int companyId)
            {
                try
                {
                    var company = await _context.companies.FindAsync(companyId);
                    if (company == null)
                    {
                        // TODO: Consider creating a new company instead of returning false
                        return false;
                    }
                    await _context.games.AddAsync(game);
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
          
                    Console.WriteLine(ex.Message);
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
                    _context.games.Update(game);
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
                        ? await _context.games.ToListAsync()
                        : await _context.games.Where(g => !g.IsDeleted).ToListAsync();

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
                    var game = await _context.games.FirstOrDefaultAsync(g => g.GameId == gameId && !g.IsDeleted);
                    return (game != null, game);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return (false, null);
                }
            }

            public async Task<(bool, List<Game>)> GetByCompanyAsync(int companyId)
            {
                try
                {
                    var games = await _context.games
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

            public async Task<bool> UpdateAsync(Game game)
            {
                try
                {
                    _context.games.Update(game);
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
