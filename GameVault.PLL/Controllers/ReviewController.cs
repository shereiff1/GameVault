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
        public async Task<IActionResult> Create(CreateReview review)
        {
           var result = await _reviewServices.CreateAsync(review);
           if (result.Item1)
           {
               return RedirectToAction("GetAllReviews");
           }
           else
           {
               ViewBag.ErrorMessage = result.Item2;
               return View("ERROR");
           }
        }
        public async Task<IActionResult> GetAllReviews()
        {
           var reviews = await _reviewServices.GetAllAsync();
           return View(reviews);
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
               return View("ERROR");
           }
        }
        public IActionResult Create()
        {
           return View();
         }
        public async Task<IActionResult> Update(UpdateReview review)
        {
            var result = await _reviewServices.UpdateAsync(review);
            if (result.Item1)
            {
                return RedirectToAction("GetAllReviews");
             }
            else
            {
                ViewBag.ErrorMessage = result.Item2;
                return View("ERROR");
             }
         }
    }
}
