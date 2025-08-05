
using GameVault.BLL.ModelVM.User;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IdentityResult> Login(UserLogin login)
        {
            var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, true, false);

            return result;
        }

        public async Task<IdentityResult> SignUp(UserSignUp register)
        {
            var user = new User(register.Email, register.Username, register.Password);
            var result = await userManager.CreateAsync(user, register.Password);

            if (result.Succeeded) 
                repo.AddUser(user);
            return result;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }
    }
}

        


