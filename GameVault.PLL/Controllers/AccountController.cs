
using GameVault.BLL.ModelVM.Account;
using GameVault.BLL.Services.Abstraction;
using GameVault.BLL.Services.Implementation;
using GameVault.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
            var (emailInUse, emailMessage) = await services.IsEmailInUse(register.Email);
            if (emailInUse)
            {
                ModelState.AddModelError("Email", emailMessage);
            }
            if (ModelState.IsValid)
            {
                var result  = await services.SignUp(register);
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
                var result = await services.Login(login);

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
            
            await services.Logout();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassword model)
        {
            if (ModelState.IsValid)
            {
                var result = await services.ForgetPassword(model, Request.Scheme, Url.Action);

                if (result.Success && !string.IsNullOrEmpty(result.PasswordResetLink))
                {
                    // Send email with the password reset link
                    // You'll need to inject your email service here
                    // MailSender.Mail("Password Reset", result.PasswordResetLink);

                    // For now, you can log it or handle it as needed
                    // logger.Log(LogLevel.Warning, result.PasswordResetLink);
                }

                return RedirectToAction("ConfirmForgetPassword");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid password reset link");
            }

            var model = new ResetPassword
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var result = await services.ResetPassword(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("ConfirmResetPassword");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmForgetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var result = services.ConfigureExternalLogin(provider, redirectUrl);
            return Challenge(result.properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login");
            }

            var result = await services.HandleExternalLoginCallback();

            if (result.Success)
            {
                return RedirectToLocal(returnUrl);
            }
            else if (result.RequiresUserCreation)
            {
                var createResult = await services.CreateExternalUser(result.ExternalLoginInfo);
                if (createResult.Success)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View("Login");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
