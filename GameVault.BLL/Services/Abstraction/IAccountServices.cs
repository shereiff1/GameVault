using GameVault.BLL.ModelVM.User;
using Microsoft.AspNetCore.Identity;

public interface IAccountServices
{
    Task<IdentityResult> Login(UserLogin user); 
    Task<IdentityResult> SignUp(UserSignUp user);
    Task Logout();
}