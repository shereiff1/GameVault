using AutoMapper;
using GameVault.BLL.Helpers.UploadFile;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entites;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.BLL.Services.Implementation
{
    public class GameServices : IGameServices
    {
        private readonly IGameRepo _gameRepo;
        private readonly IMapper _mapper;

        public GameServices(IGameRepo gameRepo, IMapper mapper)
        {
            _gameRepo = gameRepo;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(GameVM gameDto)
        {
            try
            {
                string Path = Upload.UploadFile("Files",gameDto.formFile);
                Game game = new Game(gameDto.Title, gameDto.CompanyId, gameDto.CreatedBy, gameDto.Description ,Path);
                return await _gameRepo.AddAsync(game, gameDto.Price);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<(bool, List<GameVM>?)> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var result = await _gameRepo.GetAllAsync(includeDeleted);
                bool success = result.Item1;
                List<Game>? games = result.Item2;

                return (success, games != null ? _mapper.Map<List<GameVM>>(games) : null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<(bool, EditGame?)> GetByIdAsync(int gameId)
        {
            try
            {
                var result = await _gameRepo.GetByIdWithPriceAsync(gameId);
                bool success = result.Item1;
                Game? game = result.Item2;
                decimal price = result.Item3;

                if (!success || game == null)
                    return (false, null);

                var editGame = new EditGame
                {
                    GameId = game.GameId,
                    Title = game.Title,
                    Description = game.Description,
                    CompanyId = game.CompanyId,
                    CreatedBy = game.CreatedBy,
                    Price = price
                };

                return (true, editGame);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<bool> UpdateAsync(EditGame editGame)
        {
            try
            {
                var result = await _gameRepo.GetByIdAsync(editGame.GameId);
                bool success = result.Item1;
                Game? existingGame = result.Item2;
                if (!success || existingGame == null)
                    return false;
                var updatedGame = new Game(editGame.Title, editGame.CompanyId, existingGame.CreatedBy, editGame.Description, editGame.ImagePath);
                existingGame.Update(editGame.Title, editGame.Description);
                existingGame.UpdateCompany(editGame.CompanyId);

                return await _gameRepo.UpdateAsync(existingGame, editGame.Price);
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
                return await _gameRepo.DeleteAsync(gameId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<(bool, List<GameVM>)> GetByCompanyAsync(int companyId)
        {
            try
            {
                var result = await _gameRepo.GetByCompanyAsync(companyId);
                bool success = result.Item1;
                List<Game> games = result.Item2;

                return (success, _mapper.Map<List<GameVM>>(games));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new List<GameVM>());
            }
        }
        public async Task<(bool, List<GameDetails>?)> GetAllGameDetailsAsync()
        {
            try
            {
                var (success, gameDetailsDTOs) = await _gameRepo.GetAllGameDetailsAsync();

                if (!success || gameDetailsDTOs == null)
                    return (false, null);

                var gameDetails = _mapper.Map<List<GameDetails>>(gameDetailsDTOs);
                return (true, gameDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting game details: {ex.Message}");
                return (false, null);
            }
        }
       
    }
}
