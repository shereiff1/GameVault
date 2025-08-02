using GameVault.BLL.ModelVM.Category;
using GameVault.DAL.Entities;
using GameVault.DAL.Repo.Abstraction;
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

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateCategory category)
        //{
        //    var categoryEntity = new Category
        //    {
        //        Category_Name = category.Category_Name,
        //        Description = category.Description,
        //        CreatedBy = "CurrentUser",
        //        CreatedDate = DateTime.Now
        //    };

        //    var result = await _categoryRepo.CreateAsync(categoryEntity);
        //    if (result.Item1)
        //    {
        //        return RedirectToAction("GetAllCategories");
        //    }
        //    else
        //    {
        //        ViewBag.ErrorMessage = result.Item2;
        //        return View("ERROR");
        //    }
        //}

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

        [HttpPost]
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

        //[HttpPost]
        //public async Task<IActionResult> Update(UpdateCategory category)
        //{

        //    var result = await _categoryRepo.UpdateAsync(category);


        //    if (result.Item1)
        //    {
        //        return RedirectToAction("GetAllCategories");
        //    }
        //    else
        //    {
        //        ViewBag.ErrorMessage = result.Item2;
        //        return View("ERROR");
        //    }
        //}
    }
}