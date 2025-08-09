
using System;
using System.Security.Claims;
using GameVault.BLL.ModelVM.Account;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace GameVault.BLL.Services.Implementation
{
    public class AccountServices : IAccountServices
    {
        private readonly IUserRepo repo;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountServices(IUserRepo repo, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> Login(UserLogin login)
        {
            var result = await signInManager.PasswordSignInAsync(
                login.Email,
                login.Password,
                isPersistent: true,
                lockoutOnFailure: false);

            return result.Succeeded
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError { Description = "Invalid login attempt" });
        }

        public async Task<(bool,string?)> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return (false,null);
            }
            return (true, "Email is already is taken, use a different email");

        }
     
        public async Task<IdentityResult> SignUp(UserSignUp register)
        {
            var user = new User
            {
                UserName = register.Username,
                Email = register.Email,
            };

            var result = await userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
                await repo.AddUser(user);
            return result;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<(bool Success, string PasswordResetLink)> ForgetPassword(ForgetPassword model, string scheme, Func<string, string, object, string, string> urlAction)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResetLink = urlAction("ResetPassword", "Account", new { Email = model.Email, Token = token }, scheme);
                return (true, passwordResetLink);
            }

            return (true, string.Empty);
        }

        public async Task<IdentityResult> ResetPassword(ResetPassword model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                return result;
            }

            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }



        public (AuthenticationProperties properties, string provider) ConfigureExternalLogin(string provider, string redirectUrl)
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return (properties, provider);
        }

        public async Task<ExternalLoginResult> HandleExternalLoginCallback()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return new ExternalLoginResult
                {
                    Success = false,
                    Errors = { "Error loading external login information." }
                };
            }

            var result = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return new ExternalLoginResult { Success = true };
            }
            else if (result.IsLockedOut)
            {
                return new ExternalLoginResult
                {
                    Success = false,
                    Errors = { "User account locked out." }
                };
            }
            else
            {
                return new ExternalLoginResult
                {
                    Success = false,
                    RequiresUserCreation = true,
                    ExternalLoginInfo = info
                };
            }
        }

        public async Task<ExternalLoginCreationResult> CreateExternalUser(ExternalLoginInfo info)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(email))
            {
                return new ExternalLoginCreationResult
                {
                    Success = false,
                    Errors = { "Email claim not received from external provider." }
                };
            }

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                var addLoginResult = await userManager.AddLoginAsync(existingUser, info);
                if (addLoginResult.Succeeded)
                {
                    await signInManager.SignInAsync(existingUser, isPersistent: false);
                    return new ExternalLoginCreationResult { Success = true };
                }
                else
                {
                    return new ExternalLoginCreationResult
                    {
                        Success = false,
                        Errors = addLoginResult.Errors.Select(e => e.Description).ToList()
                    };
                }
            }

            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await repo.AddUser(user);

                result = await userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return new ExternalLoginCreationResult { Success = true };
                }
            }

            return new ExternalLoginCreationResult
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
}

        


