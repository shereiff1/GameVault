
using GameVault.BLL.ModelVM.Account;
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
            var (emailInUse, emailMessage) = await services.IsEmailInUse(register.Email);
            if (emailInUse)
            {
                ModelState.AddModelError("Email", emailMessage);
            }
            if (ModelState.IsValid)
            {
                var confirmationUrl = Url.Action("ConfirmEmail", "Account", null, Request.Scheme);
                var result = await services.SignUp(register, confirmationUrl);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Registration successful! Please check your email to confirm your account before logging in.";
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
                    var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Invalid UserName Or Password";

                    if (errorMessage.Contains("confirm your email"))
                    {
                        ViewBag.ShowResendLink = true;
                        ViewBag.Email = login.Email;
                    }
                    ModelState.AddModelError("", errorMessage);

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
                try
                {
                    var result = await services.ForgetPassword(model, Request.Scheme, Url.Action);

                    if (result.Success)
                    {
                        TempData["Message"] = "If your email is registered with us, you will receive a password reset link shortly.";
                        return RedirectToAction("ConfirmForgetPassword");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            ModelState.AddModelError("", result.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while processing your request. Please try again.";
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Invalid password reset link.";
                return RedirectToAction("Login");
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
                try
                {
                    var result = await services.ResetPassword(model);

                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Your password has been successfully reset. You can now sign in with your new password.";
                        return RedirectToAction("ConfirmResetPassword");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while resetting your password. Please try again.");
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
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "Invalid email confirmation link.";
                ViewBag.Success = false;
                return View();
            }

            var result = await services.ConfirmEmail(userId, token);

            if (result.Succeeded)
            {
                ViewBag.Message = "Email confirmed successfully! You can now log in.";
                ViewBag.Success = true;
            }
            else
            {
                ViewBag.Message = "Error confirming email: " + string.Join(", ", result.Errors.Select(e => e.Description));
                ViewBag.Success = false;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Email is required.";
                return RedirectToAction("Login");
            }

            var confirmationUrl = Url.Action("ConfirmEmail", "Account", null, Request.Scheme);
            var result = await services.ResendConfirmationEmail(email, confirmationUrl);

            if (result.Succeeded)
            {
                TempData["Message"] = "Confirmation email has been resent. Please check your email.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("Login");
        }
    }
}
