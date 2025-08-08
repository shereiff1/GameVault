using GameVault.BLL.ModelVM.Category;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.Services.Implementation;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace GameVault_PLL.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryservices)
        {
            _categoryServices = categoryservices;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategory category)
        {
            //check
            var result = await _categoryServices.CreateAsync(category);
            if (result.Item1)
            {
                return RedirectToAction("GetAllCategories");
                //return Json(new { success = true, redirectUrl = Url.Action("GetAllCategories") });

            }
            else
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("ERROR");
                //return Json(new { success = false, errorMessage = result.Item2, data = category });

            }
        }

        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryServices.GetAllAsync();
            if (result.Item2 == null)
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("No data to get ");
            }
            else
            {
                return View(result.Item2);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryServices.DeleteAsync(id);
            if (result.Item1)
            {
                return RedirectToAction("GetAllCategories");
            }
            else
            {
                return RedirectToAction("GetAllCategories", new { errorMessage = "Category deletion failed!" });
            }
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            var result = await _categoryServices.GetByIdAsync(id);
            if (result.Item2!=null)
            {
                return View(result.Item2);
            }
            else
            {
                return RedirectToAction("GetAllCategories", new { errorMessage = "Category not found!" });

            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategory category)
        {

            var result = await _categoryServices.UpdateAsync(category);


            if (result.Item1)
            {
                return RedirectToAction("GetAllCategories");
            }
            else
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("ERROR");
            }
        }

    }
}
