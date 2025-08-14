using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.Services.Abstraction;

namespace GameVault.PLL.Services
{
    public class FeaturedGameBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FeaturedGameBackgroundService> _logger;

        private static List<GameDetails> _allGames = new();
        private static int _currentGameIndex = 0;

        public static GameDetails? CurrentFeaturedGame { get; private set; }
        public static DateTime LastUpdate { get; private set; } = DateTime.MinValue;

        public FeaturedGameBackgroundService(IServiceScopeFactory scopeFactory, ILogger<FeaturedGameBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
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
                    UpdateFeaturedGame();
                    await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Featured Game Background Service");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
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
                        CurrentFeaturedGame = _allGames[0];
                        LastUpdate = DateTime.UtcNow;
                    }
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

        private void UpdateFeaturedGame()
        {
            if (DateTime.UtcNow - LastUpdate > TimeSpan.FromHours(1))
            {
                _ = LoadGames(); // refresh games list in the background
            }

            if (_allGames.Count == 0)
                return;

            _currentGameIndex = (_currentGameIndex + 1) % _allGames.Count;
            CurrentFeaturedGame = _allGames[_currentGameIndex];
            LastUpdate = DateTime.UtcNow;

            _logger.LogInformation($"Updated featured game to: {CurrentFeaturedGame?.Title}");
        }

        public static GameDetails? GetCurrentFeaturedGame() => CurrentFeaturedGame;
    }
}
