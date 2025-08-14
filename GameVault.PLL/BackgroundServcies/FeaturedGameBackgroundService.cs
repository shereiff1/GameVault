using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.Services.Abstraction;
using GameVault.PLL.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GameVault.PLL.Services
{
    public class FeaturedGameBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FeaturedGameBackgroundService> _logger;
        private readonly IHubContext<FeaturedGameHub> _hubContext; 

        private static List<GameDetails> _allGames = new();
        private static int _currentGameIndex = 0;

        public static GameDetails? CurrentFeaturedGame { get; private set; }
        private static DateTime _lastReload = DateTime.MinValue;

        public FeaturedGameBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<FeaturedGameBackgroundService> logger,
            IHubContext<FeaturedGameHub> hubContext
        )
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Featured Game Background Service started");

            await LoadGames();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateFeaturedGame();
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Featured Game Background Service");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            _logger.LogInformation("Featured Game Background Service stopped");
        }

        private async Task LoadGames()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var gameServices = scope.ServiceProvider.GetRequiredService<IGameServices>();

                var (success, games) = await gameServices.GetAllGameDetailsAsync();
                if (success && games != null && games.Any())
                {
                    _allGames = games.ToList();
                    _logger.LogInformation($"Loaded {_allGames.Count} games for featured rotation");

                    if (CurrentFeaturedGame == null)
                    {
                        _currentGameIndex = 0;
                        CurrentFeaturedGame = _allGames[_currentGameIndex];
                    }

                    _lastReload = DateTime.UtcNow;
                }
                else
                {
                    _logger.LogWarning("No games found for featured rotation");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load games");
            }
        }

        private async Task UpdateFeaturedGame()
        {
            if (DateTime.UtcNow - _lastReload > TimeSpan.FromHours(1))
                await LoadGames();

            if (_allGames.Count == 0)
                return;

            _currentGameIndex = (_currentGameIndex + 1) % _allGames.Count;
            CurrentFeaturedGame = _allGames[_currentGameIndex];

            _logger.LogInformation($"Updated featured game to: {CurrentFeaturedGame?.Title}");

            if (CurrentFeaturedGame != null)
            {
                var dto = new
                {
                    GameId = CurrentFeaturedGame.GameId,
                    Title = CurrentFeaturedGame.Title,
                    ImagePath = CurrentFeaturedGame.ImagePath,
                    CompanyName = CurrentFeaturedGame.CompanyName,
                    Description = CurrentFeaturedGame.Description,
                    Price = CurrentFeaturedGame.Price,
                    Rating = CurrentFeaturedGame.Rating,
                    Categories = CurrentFeaturedGame.Categories?.Select(c => new { c.Category_Name }).ToList(),
                    Reviews = CurrentFeaturedGame.Reviews?.Select(r => new { r.Comment }).ToList()
                };

                await _hubContext.Clients.All.SendAsync("UpdateFeaturedGame", dto);
            }
        }


        public static GameDetails? GetCurrentFeaturedGame() => CurrentFeaturedGame;
    }
}
