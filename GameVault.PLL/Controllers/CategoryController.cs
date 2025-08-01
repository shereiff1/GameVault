using GameVault_BLL.ModelVM.Category;
using Microsoft.AspNetCore.Mvc;

namespace GameVault_PLL.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _categoryRepo;
        public CategoryController(ICategoryRepo categoryRepo)
        {
           _categoryRepo = categoryRepo;
        }
          public IActionResult Create()
        {
           return View();
        }
        public async Task<IActionResult> Create(CreateCategory category)
        {
           var result = await _categoryRepo.CreateAsync(category);
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
        public async Task<IActionResult> GetAllCategories()
        {
           var result = await _categoryRepo.GetAllAsync();
           if (result.Item2 == null)
           {
               return View(result.Item1);
            }
           else
           {
               ViewBag.ErrorMessage = result.Item2;
               return View("ERROR");
             }
          }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryRepo.DeleteAsync(id);
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
        public async Task<IActionResult> Update(UpdateCategory category)
        {
           var result = await _categoryRepo.UpdateAsync(category);
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
