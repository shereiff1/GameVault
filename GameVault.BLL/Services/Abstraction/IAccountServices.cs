

using GameVault.BLL.ModelVM.User;
using GameVault.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IAccountServices
    {
        Task<IdentityResult> Login(UserLogin user);

        Task<IdentityResult> SignUp(UserSignUp user);

        Task Logout();

    }
}
