using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Review;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameVault_PLL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReviewController : Controller
    {
        private readonly IReviewServices _reviewServices;
        public ReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }


        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReview review)
        {
            try
            {
                if (review == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        errorMessage = "No data received",
                        receivedData = Request.Body
                    });
                }
                var result = await _reviewServices.CreateAsync(review);
                if (result.Item1)
                {
                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("GetAllReviews")
                    });
                }
                return BadRequest(new
                {
                    success = false,
                    errorMessage = result.Item2,
                    data = review
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
                }
                );
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewServices.GetAllAsync();
            if (reviews.Item2 == null)
            {
                ViewBag.ErrorMessage = reviews.Item2;
                return View("No data to get ");
            }
            return View(reviews.Item2);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewServices.DeleteAsync(id);
            if (result.Item1)
            {
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("GetAllReviews")
                });
            }
            return Json(new
            {
                success = false,
                errorMessage = result.Item2,
                data = id
            });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewDTO review)
        {
            var result = await _reviewServices.UpdateAsync(review);
            if (result.Item1)
            {
                return Json(new
                {
                    success = true,
                    message = review,
                    redirectUrl = Url.Action("GetAllReviewss")
                });
            }
            return BadRequest(new
            {
                success = false,
                errorMessage = result.Item2,
                data = review
            });
        }
        public async Task<IActionResult> Update(int id)
        {
            var result = await _reviewServices.GetByIdAsync(id);
            if (result.Item2 != null)
            {
                return View(result.Item2);
            }
            else
            {
                return RedirectToAction("GetAllReviews", new { errorMessage = "Review not found!" });

            }
        }

    }
}

