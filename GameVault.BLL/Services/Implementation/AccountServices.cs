
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
        private readonly IEmailService emailService;

        public AccountServices(IUserRepo repo, UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
        }

        public async Task<IdentityResult> Login(UserLogin login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid login attempt" });
            }
            if (!user.EmailConfirmed)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Please confirm your email before logging in" });
            }
            var result = await signInManager.PasswordSignInAsync(
                login.Email,
                login.Password,
                isPersistent: true,
                lockoutOnFailure: false);


            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }
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
     
        public async Task<IdentityResult> SignUp(UserSignUp register, string confirmationUrl)
        {
            var (emailInUse, emailMessage) = await IsEmailInUse(register.Email);
            if (emailInUse)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "Email already in use",
                    Description = emailMessage
                });
            }

            var user = new User
            {
                UserName = register.Username,
                Email = register.Email,
                EmailConfirmed = false
            };

            var result = await userManager.CreateAsync(user, register.Password);



            if (result.Succeeded)
            {
                await repo.AddUser(user);
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = confirmationUrl + $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";
                await emailService.SendEmailAsync(
                            user.Email,
                            "Confirm Your Email",
                            $@"
                <h2>Welcome to Our App!</h2>
                <p>Please confirm your email address by clicking the link below:</p>
                <p><a href='{confirmationLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Confirm Email</a></p>
                <p>If the button doesn't work, copy and paste this link into your browser:</p>
                <p>{confirmationLink}</p>
                <p>This link will expire in 24 hours.</p>
                ");
                await userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<(bool Success, string PasswordResetLink, string ErrorMessage)> ForgetPassword(
        ForgetPassword model, string scheme, Func<string, string, object, string, string> urlAction)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var resetLink = urlAction("ResetPassword", "Account",
                        new { email = user.Email, token = Uri.EscapeDataString(token) }, scheme);

                    // Send email
                    await emailService.SendEmailAsync(
                        user.Email,
                        "Reset Your Password",
                        $@"
                <h2>Password Reset Request</h2>
                <p>You have requested to reset your password. Click the link below to create a new password:</p>
                <p><a href='{resetLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Reset Password</a></p>
                <p>If the button doesn't work, copy and paste this link into your browser:</p>
                <p><a href='{resetLink}'>{resetLink}</a></p>
                <p><strong>Important:</strong></p>
                <ul>
                    <li>This link will expire in 1 hour for security reasons</li>
                    <li>If you didn't request this password reset, please ignore this email</li>
                    <li>Your password will remain unchanged until you create a new one</li>
                </ul>
                <p>If you continue to have problems, please contact our support team.</p>
                ");

                    return (true, resetLink, null);
                }

                return (true, null, null);
            }
            catch (Exception ex)
            {
                return (false, null, "An error occurred while processing your request.");
            }
        }

        public async Task<IdentityResult> ResetPassword(ResetPassword model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Invalid password reset request."
                    });
                }

                var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    try
                    {
                        await emailService.SendEmailAsync(
                            user.Email,
                            "Password Changed Successfully",
                            $@"
                    <h2>Password Changed</h2>
                    <p>Your password has been successfully changed.</p>
                    <p>If you did not make this change, please contact our support team immediately.</p>
                    <p>For security reasons, you may want to:</p>
                    <ul>
                        <li>Sign in with your new password to verify it works</li>
                        <li>Review your account activity</li>
                        <li>Update your password on any other accounts where you used the same password</li>
                    </ul>
                    ");
                    }
                    catch (Exception emailEx)
                    {
                        
                        return IdentityResult.Failed(new IdentityError
                        {
                            Description = "Password changed but failed to send confirmation email."
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "An error occurred while resetting your password."
                });
            }
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
                    await userManager.AddToRoleAsync(user, "User");
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
        public async Task<IdentityResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task<IdentityResult> ResendConfirmationEmail(string email, string confirmationUrl)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            if (user.EmailConfirmed)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email is already confirmed" });
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = confirmationUrl + $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await emailService.SendEmailAsync(

                user.Email,
                "Confirm Your Email - Resend",
                $@"
        <h2>Email Confirmation</h2>
        <p>Please confirm your email address by clicking the link below:</p>
        <p><a href='{confirmationLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Confirm Email</a></p>
        <p>If the button doesn't work, copy and paste this link into your browser:</p>
        <p>{confirmationLink}</p>
        ");

            return IdentityResult.Success;
        }
    }
}

        


