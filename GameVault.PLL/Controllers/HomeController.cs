using System.Diagnostics;
using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.Services.Abstraction;
using GameVault.PLL.Models;
using GameVault.PLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.PLL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameServices gameServices;
        private readonly IFeaturedGameService featuredGameService;

        public HomeController(ILogger<HomeController> logger, IGameServices gameServices, IFeaturedGameService featuredGameService)
        {
            _logger = logger;
            this.gameServices = gameServices;
            this.featuredGameService = featuredGameService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var (success, gameDetails) = await gameServices.GetAllGameDetailsAsync();
                var games = success && gameDetails != null ? gameDetails : new List<GameDetails>();
                var featuredGame = await featuredGameService.GetCurrentFeaturedGameAsync();
                ViewBag.FeaturedGame = featuredGame;
                return View(games);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading home page");
                ViewBag.FeaturedGame = null;
                return View(new List<GameDetails>());
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