
using GameVault.BLL.ModelVM.User;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.Services.Implementation;
using GameVault.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.PLL.Controllers
{
    public class AccountController : Controller
    {   
        private readonly IAccountServices services;
        public AccountController(IAccountServices services)
        {
            this.services = services;
         
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUp register)
        {
            if(ModelState.IsValid)
            {
                var result  = services.SignUp(register).Result;
                if (result.Succeeded)
                {

                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("Password", item.Description);
                    }
                } 
            }
            return View(register);
            
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                var result = services.Login(login).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName Or Password");

                }
            }
            return View(login);            
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            
             services.Logout();
            return RedirectToAction("Login");
        }
    }
}
