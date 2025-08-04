using System.Diagnostics;
using GameVault.BLL.Services.Abstraction;
using GameVault.PLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.PLL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameServices gameServices;

        public HomeController(ILogger<HomeController> logger, IGameServices gameServices)
        {
            _logger = logger;
            this.gameServices = gameServices;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var (success, gameDetails) = await gameServices.GetAllGameDetailsAsync();

                if (success && gameDetails != null)
                {
                    return View(gameDetails);
                }
                else
                {
                    _logger.LogWarning("Failed to retrieve game details for home page");
                    return View(new List<GameVault.BLL.ModelVM.Game.GameDetails>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading home page");
                return View(new List<GameVault.BLL.ModelVM.Game.GameDetails>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
