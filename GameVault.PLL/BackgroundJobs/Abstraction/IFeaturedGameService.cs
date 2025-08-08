using GameVault.BLL.ModelVM.Game;

namespace GameVault.PLL.Services
{
    public interface IFeaturedGameService
    {
        GameDetails? GetCurrentFeaturedGame();
        Task<GameDetails?> GetCurrentFeaturedGameAsync();
    }

    public class FeaturedGameService : IFeaturedGameService
    {
        private readonly ILogger<FeaturedGameService> _logger;

        public FeaturedGameService(ILogger<FeaturedGameService> logger)
        {
            _logger = logger;
        }

        public GameDetails? GetCurrentFeaturedGame()
        {
            return FeaturedGameBackgroundService.GetCurrentFeaturedGame();
        }

        public async Task<GameDetails?> GetCurrentFeaturedGameAsync()
        {
            // For now, this is synchronous, but you could add async logic if needed
            return await Task.FromResult(GetCurrentFeaturedGame());
        }
    }
}