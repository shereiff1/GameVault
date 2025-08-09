using System.Security.Claims;
using AutoMapper;
using GameVault.BLL.Interfaces;
using GameVault.BLL.ModelVM.Roles;
using GameVault.BLL.ModelVM.User;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GameVault.BLL.Services
{
    public class RoleServices : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserRepo userRepo;

        public RoleServices(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IMapper mapper,
            ILogger<RoleServices> logger,IUserRepo userRepo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            this.userRepo = userRepo;
        }

        public async Task<IdentityResult> CreateRoleAsync(CreateRoleVM model)
        {
            try
            {

                var identityRole = new IdentityRole
                {
                    Name = model.RoleName,
                    
                };

                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    await _roleManager.AddClaimAsync(identityRole,
                    new Claim("CreatedBy", model.CreatedBy ?? "Unknwon"));
                    await _roleManager.AddClaimAsync(identityRole,
                        new Claim("CreatedOn", (model.CreatedOn ?? DateTime.Now).ToString("O")));
                }
                

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<RoleVM>?> GetAllRoles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                var rolesWithMetadata = new List<RoleVM>();

                foreach (var role in roles)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    var createdBy = claims.FirstOrDefault(c => c.Type == "CreatedBy")?.Value;
                    var createdOnStr = claims.FirstOrDefault(c => c.Type == "CreatedOn")?.Value;


                    DateTime? createdOn = null;

                    if (DateTime.TryParse(createdOnStr, out DateTime parsedCreatedOn))
                        createdOn = parsedCreatedOn;

                    

                    rolesWithMetadata.Add(new RoleVM
                    {
                        Id = role.Id,
                        Name = role.Name,
                        CreatedBy = createdBy,
                        CreatedOn = createdOn
                    });
                }

                return rolesWithMetadata.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<UserRoleVM>?> GetUsersInRoleAsync(string roleId)
        {
            try
            {

                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    return null;
                }

                var model = new List<UserRoleVM>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    if (user == null) continue;
                    var userRoleViewModel = new UserRoleVM
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        ProfilePicture = user.ProfilePicture,
                        CreatedOn = user.CreatedOn
                    };

        
                    try
                    {
                        userRoleViewModel.IsSelected = await _userManager.IsInRoleAsync(user,role.Name);
                    }
                    catch (Exception ex)
                    {
                        userRoleViewModel.IsSelected = false; 
                    }
                    model.Add(userRoleViewModel);
                }


                return model;
            }
            catch (Exception ex)
            {
                return new List<UserRoleVM>();
            }
        }

        public async Task<IdentityResult> UpdateUsersInRoleAsync(List<UserRoleVM> model, string roleId)
        {
            try
            {

                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                    });
                }

                var overallResult = IdentityResult.Success;

                foreach (var userRole in model)
                {
                    var user = await _userManager.FindByIdAsync(userRole.UserId);
                    if (user == null)
                    {
                        continue;
                    }

                    var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                    IdentityResult result = null;

                    if (userRole.IsSelected && !isInRole)
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                        if (result.Succeeded)
                        {
                        }
                    }
                    else if (!userRole.IsSelected && isInRole)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                        if (result.Succeeded)
                        {
                            
                        }
                    }

                    if (result != null && !result.Succeeded)
                    {

                        // Combine errors from failed operations
                        var errors = overallResult.Errors.Concat(result.Errors).ToArray();
                        overallResult = IdentityResult.Failed(errors);
                    }
                }

                return overallResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                }

                return role;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> RoleExistsAsync(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                return role != null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}