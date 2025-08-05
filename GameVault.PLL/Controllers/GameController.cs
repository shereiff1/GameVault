using Microsoft.AspNetCore.Mvc;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Game;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameVault.PLL.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameServices _gameServices;
        private readonly ICompanyServices _companyServices;

        public GameController(IGameServices gameServices, ICompanyServices companyServices)
        {
            _gameServices = gameServices;
            _companyServices = companyServices;
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

        public async Task<IActionResult> Add(int? companyId = null)
        {
            var (success, companies) = await _companyServices.GetAllAsync();
            if (!success || companies == null)
            {
                ViewBag.Error = "Failed to load companies.";
                ViewBag.Companies = new SelectList(new List<CompanyVM>(), "CompanyId", "CompanyName");
            }
            else
            {
                ViewBag.Companies = new SelectList(companies, "CompanyId", "CompanyName", companyId);
            }

            var gameVm = new GameVM();
            if (companyId.HasValue)
            {
                gameVm.CompanyId = companyId.Value;
            }

            return View(gameVm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GameVM game)
        {
            if (ModelState.IsValid)
            {
                if (await _gameServices.AddAsync(game))
                    return RedirectToAction("Index");
            }

            var (success, companies) = await _companyServices.GetAllAsync();
            ViewBag.Companies = success && companies != null
                ? new SelectList(companies, "CompanyId", "CompanyName", game.CompanyId)
                : new SelectList(new List<CompanyVM>(), "CompanyId", "CompanyName");

            ViewBag.Error = "Failed to add game!";
            return View(game);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var (success, game) = await _gameServices.GetByIdAsync(id);
            if (!success || game == null)
                return RedirectToAction("Index", new { errorMessage = "Game not found!" });

            var (companySuccess, companies) = await _companyServices.GetAllAsync();
            ViewBag.Companies = companySuccess && companies != null
                ? new SelectList(companies, "CompanyId", "CompanyName", game.CompanyId)
                : new SelectList(new List<CompanyVM>(), "CompanyId", "CompanyName");

            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGame game)
        {
            if (ModelState.IsValid)
            {
                if (await _gameServices.UpdateAsync(game))
                    return RedirectToAction("Index");
            }

            var (success, companies) = await _companyServices.GetAllAsync();
            ViewBag.Companies = success && companies != null
                ? new SelectList(companies, "CompanyId", "CompanyName", game.CompanyId)
                : new SelectList(new List<CompanyVM>(), "CompanyId", "CompanyName");

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

        public IActionResult CreateCompany()
        {
            return RedirectToAction("Add", "Company", new { returnToGame = true });
        }
    }
}
