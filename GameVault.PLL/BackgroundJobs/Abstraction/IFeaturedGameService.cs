using GameVault.BLL.ModelVM.Game;

namespace GameVault.PLL.Services
{
    public interface IFeaturedGameService
    {
        GameDetails? GetCurrentFeaturedGame();
        Task<GameDetails?> GetCurrentFeaturedGameAsync();
    }
}