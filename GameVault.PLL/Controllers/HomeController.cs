using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.Services.Abstraction;
using GameVault.PLL.Models;
using GameVault.PLL.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static GameVault.PLL.Services.SaleBackgroundService;

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

        [HttpGet]
        public IActionResult GetFeaturedGamePartial()
        {
            var featuredGame = FeaturedGameBackgroundService.GetCurrentFeaturedGame();
            if (featuredGame == null)
                return NotFound();

            ViewBag.IsSaleActive = SaleStatus.IsSaleActive;
            return PartialView("_FeaturedGamePartial", featuredGame);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var (success, gameDetails) = await gameServices.GetAllGameDetailsAsync();
                var games = success && gameDetails != null ? gameDetails : new List<GameDetails>();

                if (SaleStatus.IsSaleActive)
                {
                    foreach (var game in games)
                    {
                        game.Price *= 0.8m;
                    }
                }

                var featuredGame = FeaturedGameBackgroundService.GetCurrentFeaturedGame();
                ViewBag.FeaturedGame = featuredGame;
                ViewBag.IsSaleActive = SaleStatus.IsSaleActive;

                return View(games);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading home page");
                ViewBag.FeaturedGame = null;
                return View(new List<GameDetails>());
            }
        }

        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
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
