using GameVault.BLL.Interfaces;
using GameVault.BLL.ModelVM.Roles;
using GameVault.BLL.ModelVM.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameVault.PLL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {
        private readonly IRoleService _RoleServices;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            IRoleService RoleServices,
            ILogger<RoleController> logger)
        {
            _RoleServices = RoleServices;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            var model = new CreateRoleVM
            {
                CreatedBy = User.Identity?.Name ?? "Unknown"
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Ensure CreatedBy is set if empty
                if (string.IsNullOrEmpty(model.CreatedBy))
                {
                    model.CreatedBy = User.Identity?.Name ?? "System";
                }

                var result = await _RoleServices.CreateRoleAsync(model);
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Role '{model.RoleName}' created successfully.";
                    return RedirectToAction("RolesList");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role in controller");
                ModelState.AddModelError("", "An error occurred while creating the role. Please try again.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RolesList()
        {
            try
            {
                var roles = await _RoleServices.GetAllRoles();
                return View(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving roles list in controller");
                ViewBag.ErrorMessage = "An error occurred while retrieving the roles list.";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                ViewBag.ErrorMessage = "Role ID cannot be null or empty";
                return View();
            }

            try
            {
                ViewBag.RoleId = Id;

                var role = await _RoleServices.GetRoleByIdAsync(Id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                    return View();
                }

                var model = await _RoleServices.GetUsersInRoleAsync(Id);
                if (model == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                    return View();
                }

                ViewBag.RoleName = role.Name;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users in role {RoleId}", Id);
                ViewBag.ErrorMessage = "An error occurred while retrieving the users for this role.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleVM> model, string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                ViewBag.ErrorMessage = "Role ID cannot be null or empty";
                return View();
            }

            if (model == null || !model.Any())
            {
                TempData["WarningMessage"] = "No users were provided for role assignment.";
                return RedirectToAction("EditUsersInRole", new { Id });
            }

            try
            {
                var roleExists = await _RoleServices.RoleExistsAsync(Id);
                if (!roleExists)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                    return View();
                }

                var result = await _RoleServices.UpdateUsersInRoleAsync(model, Id);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User role assignments updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Some errors occurred: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                }

                return RedirectToAction("EditUsersInRole", new { Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating users in role {RoleId}", Id);
                TempData["ErrorMessage"] = "An error occurred while updating user role assignments.";
                return RedirectToAction("EditUsersInRole", new { Id });
            }
        }
    }
}