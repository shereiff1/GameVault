using GameVault.BLL.ModelVM.Game;

namespace GameVault.PLL.Services
{
    public interface IFeaturedGameService
    {
        GameVM? GetCurrentFeaturedGame();
        Task<GameVM?> GetCurrentFeaturedGameAsync();
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
            return await Task.FromResult(GetCurrentFeaturedGame());
        }
    }
}
