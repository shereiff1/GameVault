using GameVault.BLL.ModelVM.Roles;
using GameVault.BLL.ModelVM.User;
using Microsoft.AspNetCore.Identity;

namespace GameVault.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(CreateRoleVM model);
        public Task<List<RoleVM>?> GetAllRoles();
        Task<List<UserRoleVM>?> GetUsersInRoleAsync(string roleId);
        Task<IdentityResult> UpdateUsersInRoleAsync(List<UserRoleVM> model, string roleId);
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<bool> RoleExistsAsync(string roleId);
    }
}