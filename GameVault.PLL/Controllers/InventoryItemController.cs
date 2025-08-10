using Microsoft.AspNetCore.Mvc;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.ModelVM;

namespace GameVault.PLL.Controllers
{
    public class InventoryItemController : Controller
    {
        private readonly IInventoryItemServices _inventoryItemServices;

        public InventoryItemController(IInventoryItemServices inventoryItemServices)
        {
            _inventoryItemServices = inventoryItemServices;
        }
        public async Task<IActionResult> Index(string? errorMessage = null)
        {
            var (success, items) = await _inventoryItemServices.GetAllAsync();

            if (!success || items == null)
            {
                ViewBag.Error = "Failed to load inventory items.";
                return View(new List<InventoryItemVM>());
            }

            ViewBag.Error = errorMessage;
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var (success, item) = await _inventoryItemServices.GetByIdAsync(id);

            if (!success || item == null)
                return RedirectToAction("Index", new { errorMessage = "Inventory item not found!" });

            return View(item);
        }

        public async Task<IActionResult> ByCompany(int companyId)
        {
            var (success, items) = await _inventoryItemServices.GetByCompanyAsync(companyId);

            if (!success || items == null || items.Count == 0)
                return RedirectToAction("Index", new { errorMessage = "No inventory items found for this company!" });

            return View("Index", items);
        }

        public async Task<IActionResult> ByGame(int gameId)
        {
            var (success, items) = await _inventoryItemServices.GetByGameAsync(gameId);

            if (!success || items == null || items.Count == 0)
                return RedirectToAction("Index", new { errorMessage = "No inventory items found for this game!" });

            return View("Index", items);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!await _inventoryItemServices.DeleteAsync(id))
            {
                return RedirectToAction("Index", new { errorMessage = "Inventory item deletion failed!" });
            }
            return RedirectToAction("Index");
        }
    }
}
