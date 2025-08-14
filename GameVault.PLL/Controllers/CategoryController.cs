using GameVault.BLL.ModelVM.Category;
using GameVault.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameVault_PLL.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategory category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        errorMessage = "No data received",
                        receivedData = Request.Body
                    });
                }

                var result = await _categoryServices.CreateAsync(category);

                if (result.Item1)
                {
                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("GetAllCategories")
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    errorMessage = result.Item2,
                    data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    errorMessage = "Internal server error",
                    detailedError = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [AllowAnonymous]
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
            // var result = await _categoryServices.DeleteAsync(id);
            // if (result.Item1)
            // {
            //     return RedirectToAction("GetAllCategories");
            // }
            // else
            // {
            //     return RedirectToAction("GetAllCategories", new { errorMessage = "Category deletion failed!" });
            // }
            var result = await _categoryServices.DeleteAsync(id);
            if (result.Item1)
            {
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("GetAllCategories")
                });
            }
            return Json(new
            {
                success = false,
                errorMessage = result.Item2,
                data = id
            });
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            var result = await _categoryServices.GetByIdAsync(id);
            if (result.Item2 != null)
            {
                return View(result.Item2);
            }
            else
            {
                return RedirectToAction("GetAllCategories", new { errorMessage = "Category not found!" });

            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CategoryDTO category)
        {
          
            var result = await _categoryServices.UpdateAsync(category);

            if (result.Item1)
            {
                return Json(new
                {
                    success = true,
                    message =category,
                    redirectUrl = Url.Action("GetAllCategories")
                });
            }
            return BadRequest(new
            {
                success = false,
                errorMessage = result.Item2,
                data = category
            });

    }
}
}
