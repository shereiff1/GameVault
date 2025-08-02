using Microsoft.AspNetCore.Mvc;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.ModelVM;

namespace GameVault.PLL.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryServices _inventoryServices;
        private readonly IInventoryItemServices _inventoryItemServices;

        public InventoryController(IInventoryServices inventoryServices, IInventoryItemServices inventoryItemServices)
        {
            _inventoryServices = inventoryServices;
            _inventoryItemServices = inventoryItemServices;
        }

        public async Task<IActionResult> Index(string? errorMessage = null)
        {
            var (success, inventories) = await _inventoryServices.GetAllAsync();

            if (!success || inventories == null)
            {
                ViewBag.Error = "Failed to load inventories.";
                return View(new List<InventoryVM>());
            }

            ViewBag.Error = errorMessage;
            return View(inventories);
        }

        public IActionResult AddItem(int companyId)
        {
            ViewBag.CompanyId = companyId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int companyId, int gameId, int quantity, decimal price)
        {
            if (await _inventoryServices.AddItemAsync(companyId, gameId, quantity, price))
            {
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = companyId;
            ViewBag.Error = "Failed to add item!";
            return View();
        }

        public async Task<IActionResult> EditItem(int id)
        {
            var (success, item) = await _inventoryItemServices.GetByIdAsync(id);

            if (!success || item == null)
                return RedirectToAction("Index", new { errorMessage = "Item not found!" });

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(int inventoryItemId, int quantity, decimal price)
        {
            if (await _inventoryServices.UpdateItemAsync(inventoryItemId, quantity, price))
                return RedirectToAction("Index");

            ViewBag.Error = "Update failed!";
            var (_, item) = await _inventoryItemServices.GetByIdAsync(inventoryItemId);
            return View(item);
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            if (!await _inventoryItemServices.DeleteAsync(id))
            {
                return RedirectToAction("Index", new { errorMessage = "Item deletion failed!" });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteInventoryItem(int companyId, int inventoryItemId)
        {
            if (!await _inventoryServices.DeleteInventoryItemAsync(companyId, inventoryItemId))
            {
                return RedirectToAction("Index", new { errorMessage = "Inventory item deletion failed!" });
            }

            return RedirectToAction("Index");
        }
    }
}
