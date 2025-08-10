using Microsoft.AspNetCore.Mvc;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.ModelVM;

namespace GameVault.PLL.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyServices _companyServices;

        public CompanyController(ICompanyServices companyServices)
        {
            _companyServices = companyServices;
        }

        public async Task<IActionResult> Index(string? errorMessage = null)
        {
            var (success, companies) = await _companyServices.GetAllAsync();

            if (!success || companies == null)
            {
                ViewBag.Error = "Failed to load companies.";
                return View(new List<CompanyVM>());
            }

            ViewBag.Error = errorMessage;
            return View(companies);
        }

        public IActionResult Add(bool returnToGame = false)
        {
            ViewBag.ReturnToGame = returnToGame;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CompanyVM company, bool returnToGame = false)
        {
            ModelState.Remove("returnToGame");

            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                var result = await _companyServices.AddAsync(company);
                if (result)
                {
                    if (returnToGame)
                    {
                        var (success, companies) = await _companyServices.GetAllAsync();
                        if (success && companies != null)
                        {
                            var newCompany = companies.FirstOrDefault(c => c.CompanyName == company.CompanyName);
                            if (newCompany != null)
                                return RedirectToAction("Add", "Game", new { companyId = newCompany.CompanyId });
                        }
                        return RedirectToAction("Add", "Game");
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Error = "Failed to add company!";
            ViewBag.ReturnToGame = returnToGame;
            return View(company);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var (success, company) = await _companyServices.GetByIdAsync(id);

            if (!success || company == null)
                return RedirectToAction("Index", new { errorMessage = "Company not found!" });

            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyVM company)
        {
            if (ModelState.IsValid)
            {
                if (await _companyServices.UpdateAsync(company))
                    return RedirectToAction("Index");
            }

            ViewBag.Error = "Update failed!";
            return View(company);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!await _companyServices.DeleteAsync(id))
            {
                return RedirectToAction("Index", new { errorMessage = "Company deletion failed!" });
            }

            return RedirectToAction("Index");
        }
    }
}
