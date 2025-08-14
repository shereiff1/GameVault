using Microsoft.AspNetCore.Mvc;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.ModelVM.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace GameVault.PLL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class GameController : Controller
    {
        private readonly IGameServices _gameServices;
        private readonly ICompanyServices _companyServices;
        private readonly ICategoryServices _categoryServices;

        public GameController(IGameServices gameServices, ICompanyServices companyServices, ICategoryServices categoryServices)
        {
            _gameServices = gameServices;
            _companyServices = companyServices;
            _categoryServices = categoryServices;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? errorMessage = null)
        {
            var (success, games) = await _gameServices.GetAllAsync();

            if (!success || games == null)
            {
                ViewBag.Error = "Failed to load games.";
                return base.View(new List<BLL.ModelVM.GameVM>());
            }

            ViewBag.Error = errorMessage;
            return View(games);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GameDetails(int id)
        {
            var (success, gameDetails) = await _gameServices.GetGameDetails(id);
            if (!success || gameDetails == null)
            {
                ViewBag.Error = "Failed to load Game Details.";
            }
            return View(gameDetails);
        }

        public async Task<IActionResult> Add(int? companyId = null)
        {
            await LoadViewBagData(companyId);

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

            await LoadViewBagData(game.CompanyId);
            ViewBag.Error = "Failed to add game!";
            return View(game);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var (success, game) = await _gameServices.GetByIdAsync(id);
            if (!success || game == null)
                return RedirectToAction("Index", new { errorMessage = "Game not found!" });

            await LoadViewBagData(game.CompanyId);
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

            await LoadViewBagData(game.CompanyId);
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
        [Authorize]
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

        public IActionResult CreateCategory()
        {
            return RedirectToAction("Create", "Category", new { returnToGame = true });
        }

        [Authorize]
        private async Task LoadViewBagData(int? selectedCompanyId = null)
        {
            // Load companies
            var (companySuccess, companies) = await _companyServices.GetAllAsync();
            ViewBag.Companies = companySuccess && companies != null
                ? new SelectList(companies, "CompanyId", "CompanyName", selectedCompanyId)
                : new SelectList(new List<CompanyVM>(), "CompanyId", "CompanyName");

            // Load categories
            var (categorySuccess, categories) = await _categoryServices.GetAllAsync();
            ViewBag.Categories = categorySuccess && categories != null
                ? new MultiSelectList(categories, "Category_Id", "Category_Name")
                : new MultiSelectList(new List<CategoryDTO>(), "Category_Id", "Category_Name");
        }
    }
}