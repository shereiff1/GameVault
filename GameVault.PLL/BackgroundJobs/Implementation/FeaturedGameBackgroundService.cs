using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.ModelVM.Game;

namespace GameVault.PLL.Services
{
    public class FeaturedGameBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<FeaturedGameBackgroundService> _logger;

        public static GameDetails? CurrentFeaturedGame { get; private set; }
        public static DateTime LastUpdate { get; private set; } = DateTime.MinValue;

        private static List<GameDetails> _allGames = new();
        private static int _currentGameIndex = 0;

        public FeaturedGameBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<FeaturedGameBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
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
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }

            _logger.LogInformation("Featured Game Background Service stopped");
        }

        private async Task LoadGames()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var gameServices = scope.ServiceProvider.GetRequiredService<IGameServices>();

                var (success, gameDetails) = await gameServices.GetAllGameDetailsAsync();
                if (success && gameDetails != null && gameDetails.Any())
                {
                    _allGames = gameDetails.ToList();
                    _logger.LogInformation($"Loaded {_allGames.Count} games for featured rotation");

                    if (CurrentFeaturedGame == null && _allGames.Any())
                    {
                        CurrentFeaturedGame = _allGames[0];
                        LastUpdate = DateTime.UtcNow;
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to load games or no games available");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load games from database");
            }
        }

        private async Task UpdateFeaturedGame()
        {
            if (DateTime.UtcNow - LastUpdate > TimeSpan.FromHours(1))
            {
                await LoadGames();
            }

            if (_allGames.Count == 0)
            {
                _logger.LogWarning("No games available for featured rotation");
                return;
            }

            _currentGameIndex = (_currentGameIndex + 1) % _allGames.Count;
            CurrentFeaturedGame = _allGames[_currentGameIndex];
            LastUpdate = DateTime.UtcNow;

            _logger.LogInformation($"Updated featured game to: {CurrentFeaturedGame.Title}");
        }
        public static GameDetails? GetCurrentFeaturedGame()
        {
            return CurrentFeaturedGame;
        }
    }
}
