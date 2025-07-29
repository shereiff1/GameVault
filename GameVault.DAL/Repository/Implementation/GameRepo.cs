using GameVault.DAL.Database;
using GameVault.DAL.Entites;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.DAL.Repository.Implementation
{
    public class GameRepo : IGameRepo
    {
        private readonly ApplicationDbContext _context;

        public GameRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Game game)
        {
            try
            {
                _context.games.Add(game);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Delete(int gameId)
        {
            try
            {
                var game = _context.games.FirstOrDefault(g => g.GameId == gameId);
                if (game == null) return false;
                game.MarkAsDeleted();
                _context.games.Update(game);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public (bool, List<Game>?) GetAll(bool includeDeleted = false)
        {
            try
            {
                var result = includeDeleted ? _context.games.ToList() : _context.games.Where(g => !g.IsDeleted).ToList();
                return (true, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public (bool, Game?) GetById(int gameId)
        {
            try
            {
                var game = _context.games.FirstOrDefault(g => g.GameId == gameId && !g.IsDeleted);
                return (game != null, game);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public (bool, List<Game>) GetByCompany(int companyId)
        {
            try
            {
                var games = _context.games.Where(g => g.CompanyId == companyId && !g.IsDeleted).ToList();
                return (true, games);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new List<Game>());
            }
        }

        public bool Update(Game game)
        {
            try
            {
                _context.games.Update(game);
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
