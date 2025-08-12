

using GameVault.BLL.ModelVM.Account;
using GameVault.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IAccountServices
    {
        Task<IdentityResult> Login(UserLogin user);

        Task<(bool,string)> IsEmailInUse(string email);

        Task<IdentityResult> SignUp(UserSignUp user,string confirmationUrl);


        public Task<(bool Success, string PasswordResetLink, string ErrorMessage)> ForgetPassword(
              ForgetPassword model, string scheme, Func<string, string, object, string, string> urlAction);       
        Task<IdentityResult> ResetPassword(ResetPassword model);

        Task Logout();

        (AuthenticationProperties properties, string provider) ConfigureExternalLogin(string provider, string redirectUrl);
        Task<ExternalLoginResult> HandleExternalLoginCallback();
        Task<ExternalLoginCreationResult> CreateExternalUser(ExternalLoginInfo externalLoginInfo);

        public Task<IdentityResult> ConfirmEmail(string userId, string token);

        public Task<IdentityResult> ResendConfirmationEmail(string email, string confirmationUrl);


    }
}
