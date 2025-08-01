using Microsoft.AspNetCore.Mvc;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.ModelVM;

namespace GameVault.PLL.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameServices _gameServices;

        public GameController(IGameServices gameServices)
        {
            _gameServices = gameServices;
        }

        public async Task<IActionResult> Index(string? errorMessage = null)
        {
            var (success, games) = await _gameServices.GetAllAsync();

            if (!success || games == null)
            {
                ViewBag.Error = "Failed to load games.";
                return View(new List<GameVM>());
            }

            ViewBag.Error = errorMessage;
            return View(games);
        }

        public IActionResult Add(int companyId)
        {
            ViewBag.CompanyId = companyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(int companyId, GameVM game)
        {
            if (await _gameServices.AddAsync(companyId, game))
                return RedirectToAction("Index");

            ViewBag.CompanyId = companyId;
            ViewBag.Error = "Failed to add game!";
            return View(game);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var (success, game) = await _gameServices.GetByIdAsync(id);

            if (!success || game == null)
                return RedirectToAction("Index", new { errorMessage = "Game not found!" });
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameVM game)
        {
            if (await _gameServices.UpdateAsync(game))
                return RedirectToAction("Index");

            ViewBag.Error = "Update failed!";
            return View(game);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!await _gameServices.DeleteAsync(id))
            {
                return RedirectToAction("Index", new { errorMessage = "Game deletion failed!" });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ByCompany(int companyId)
        {
            var (success, games) = await _gameServices.GetByCompanyAsync(companyId);

            if (!success || games == null || games.Count == 0)
            {
                return RedirectToAction("Index", new { errorMessage = "No games found for this company!" });
            }

            return View("Index", games);
        }
    }
}
