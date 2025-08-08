using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Review;
using GameVault.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace GameVault_PLL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewServices _reviewServices;
        public ReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }
        public async Task<IActionResult> CreateReview(CreateReview review)
        {
            var result = await _reviewServices.CreateAsync(review);
            if (result.Item1)
            {
                return RedirectToAction("GetAllReviews");
            }
            else
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("ERROR in createreview");
            }
        }
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewServices.GetAllAsync();
            if ( reviews.Item2 == null)
            {
                ViewBag.ErrorMessage = reviews.Item2;
                return View("No data to get ");
            }
            return View(reviews.Item2);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewServices.DeleteAsync(id);
            if (result.Item1)
            {
                return RedirectToAction("GetAllReviews");
            }
            else
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("ERROR delete");
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> UpdateReview (UpdateReview review)
        {
            var result = await _reviewServices.UpdateAsync(review);
            if (result.Item1)
            {
                return RedirectToAction("GetAllReviews");
            }
            else
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("ERROR in updatereview");
            }
        }
        public async Task<IActionResult> Update(int id)
        {
            var result = await _reviewServices.GetByIdAsync(id);
            if (result.Item1)
            {
                return View(result.Item2);
            }
            else
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("ERROR in update");
            }
        }

    }
}
        
