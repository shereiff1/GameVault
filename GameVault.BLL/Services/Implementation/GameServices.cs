using AutoMapper;
using GameVault.BLL.Helpers.UploadFile;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Category;
using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.ModelVM.Review;
using GameVault.BLL.Services.Abstraction;

using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.BLL.Services.Implementation
{
    public class GameServices : IGameServices
    {
        private readonly IGameRepo _gameRepo;
        private readonly IMapper _mapper;
        private readonly IInventoryItemRepo _inventoryItemRepo;
        private readonly ICategoryServices _categoryServices;

        public GameServices(IGameRepo gameRepo, IMapper mapper, IInventoryItemRepo inventoryItem, ICategoryServices categoryServices)
        {
            _gameRepo = gameRepo;
            _mapper = mapper;
            _inventoryItemRepo = inventoryItem;
            _categoryServices = categoryServices;
        }

        public async Task<bool> AddAsync(ModelVM.GameVM gameDto)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(gameDto.NewCategoryName))
                {
                    var newCategory = new CreateCategory
                    {
                        Category_Name = gameDto.NewCategoryName,
                        Description = gameDto.NewCategoryDescription ?? string.Empty
                    };

                    var (categoryCreated, categoryMessage) = await _categoryServices.CreateAsync(newCategory);
                    if (categoryCreated)
                    {
                        var (success, categories) = await _categoryServices.GetAllAsync();
                        if (success && categories != null)
                        {
                            var createdCategory = categories.LastOrDefault(c => c.Category_Name == gameDto.NewCategoryName);
                            if (createdCategory != null)
                            {
                                gameDto.SelectedCategoryIds.Add(createdCategory.Category_Id);
                            }
                        }
                    }
                }

                string Path = Upload.UploadFile("Files", gameDto.formFile);
                Game game = new Game(gameDto.Title, gameDto.CompanyId, gameDto.CreatedBy, gameDto.Description, Path);

                return await _gameRepo.AddAsync(game, gameDto.Price, gameDto.SelectedCategoryIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<(bool, List<ModelVM.GameVM>?)> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var result = await _gameRepo.GetAllAsync(includeDeleted);
                bool success = result.Item1;
                List<Game>? games = result.Item2;

                return (success, games != null ? _mapper.Map<List<ModelVM.GameVM>>(games) : null);
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

                var categoryIds = await _gameRepo.GetGameCategoryIdsAsync(gameId);

                var editGame = new EditGame
                {
                    GameId = game.GameId,
                    Title = game.Title,
                    Description = game.Description,
                    CompanyId = game.CompanyId,
                    CreatedBy = game.CreatedBy,
                    Price = price,
                    ImagePath = game.ImagePath,
                    SelectedCategoryIds = categoryIds
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

                if (!string.IsNullOrWhiteSpace(editGame.NewCategoryName))
                {
                    var newCategory = new CreateCategory
                    {
                        Category_Name = editGame.NewCategoryName,
                        Description = editGame.NewCategoryDescription ?? string.Empty
                    };

                    var (categoryCreated, categoryMessage) = await _categoryServices.CreateAsync(newCategory);
                    if (categoryCreated)
                    {
                        var (categorySuccess, categories) = await _categoryServices.GetAllAsync();
                        if (categorySuccess && categories != null)
                        {
                            var createdCategory = categories.LastOrDefault(c => c.Category_Name == editGame.NewCategoryName);
                            if (createdCategory != null)
                            {
                                editGame.SelectedCategoryIds.Add(createdCategory.Category_Id);
                            }
                        }
                    }
                }

                string imagePath = existingGame.ImagePath;
                if (editGame.formFile != null && editGame.formFile.Length > 0)
                {
                    imagePath = Upload.UploadFile("Files", editGame.formFile);
                }
                existingGame.Update(editGame.Title, editGame.Description);
                existingGame.UpdateCompany(editGame.CompanyId);
                existingGame.UpdatePhoto(imagePath);

                return await _gameRepo.UpdateAsync(existingGame, editGame.Price, editGame.SelectedCategoryIds);
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

        public async Task<(bool, List<ModelVM.GameVM>)> GetByCompanyAsync(int companyId)
        {
            try
            {
                var result = await _gameRepo.GetByCompanyAsync(companyId);
                bool success = result.Item1;
                List<Game> games = result.Item2;

                return (success, _mapper.Map<List<ModelVM.GameVM>>(games));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, new List<ModelVM.GameVM>());
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

        public async Task<(bool success, GameDetails?)> GetGameDetails(int gameId)
        {
            try
            {
                var (success, game) = await _gameRepo.GetGameDetails(gameId);

                if (!success || game == null)
                    return (false, null);
                var (inventorySuccess, inventoryItems) = await _inventoryItemRepo.GetByGameAsync(gameId);
                var inventoryItem = inventoryItems?.FirstOrDefault();

                var details = new GameDetails
                {
                    GameId = game.GameId,
                    Title = game.Title,
                    Description = game.Description,
                    CompanyName = game.Company.CompanyName,
                    ImagePath = game.ImagePath,
                    Rating = game.Reviews != null && game.Reviews.Any()
                        ? game.Reviews.Average(r => r.Rating)
                        : 0,
                    Price = inventoryItem?.Price ?? 0,
                    Reviews = game.Reviews?.Select(r => new ReviewDTO
                    {
                        Review_Id = r.Review_Id,
                        Player_Id = r.Player_Id,
                        Game_Id = r.Game_Id,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        CreatedOn = r.CreatedOn
                    }).ToList() ?? new List<ReviewDTO>(),
                    Categories = game.Categories?.Select(c => new CategoryDTO
                    {
                        Category_Id = c.Category_Id,
                        Category_Name = c.Category_Name,
                        Description = c.Description,
                        CreatedOn = c.CreatedOn
                    }).ToList() ?? new List<CategoryDTO>()
                };

                return (true, details);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching game details: {ex.Message}");
                return (false, null);
            }
        }
    }
}