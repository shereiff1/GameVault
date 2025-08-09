using GameVault.BLL.ModelVM.User;
using GameVault.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameVault.PLL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices userServices;

        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        // GET: User/Index - Display all public users
        public async Task<IActionResult> Index()
        {
            try
            {
                var (users, error) =await userServices.GetAllUsers();
                
                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.ErrorMessage = error;
                    return View();
                }

                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading users.";
                return View();
            }
        }

        // GET: User/AllPrivateUsers - Display all private user profiles (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllPrivateUsers()
        {
            try
            {
                var (users, error) = await userServices.GetAllPrivateUsers();
                
                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.ErrorMessage = error;
                    return View();
                }

                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading private user data.";
                return View();
            }
        }

        // GET: User/Profile/{id} - Display user profile
        public async Task<IActionResult> PublicProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("User ID is required.");
            }

            try
            {
                var (user, error) = await userServices.GetPublicProfile(id);
                
                if (!string.IsNullOrEmpty(error))
                {
                    return NotFound(error);
                }
      
                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading user profile.";
                return View();
            }
        }
        public async Task<IActionResult> PrivateProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("User ID is required.");
            }

            try
            {
                var (user, error) = await userServices.GetPrivateProfile(id);

                if (!string.IsNullOrEmpty(error))
                {
                    return NotFound(error);
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading user profile.";
                return View();
            }
        }
        // GET: User/UpdateProfile - Show update profile form
        [Authorize]
        public async Task<IActionResult> UpdateProfile()
        {
            try
            {
                var userId = await GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var (currentUser, error) = await userServices.GetUserInfo(userId);
                if (!string.IsNullOrEmpty(error) || currentUser == null)
                {
                    ViewBag.ErrorMessage = error;
                    return View();
                }

                return View(currentUser);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading profile data.";
                return View();
            }
        }

        // POST: User/UpdateProfile - Handle profile update
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Ensure the user can only update their own profile
                var userId = await GetCurrentUserId();
                if (string.IsNullOrEmpty(userId) || userId != model.Id)
                {
                    return Forbid("You can only update your own profile.");
                }
                var (success, error) = await userServices.UpdateUserProfile(model);

                if (!success)
                {
                    ViewBag.ErrorMessage = error ?? "Failed to update profile.";
                    return View(model);
                }
                return RedirectToAction("PrivateProfile", new { id = model.Id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while updating your profile.";
                return View(model);
            }
        }

        // GET: User/Search - Search users
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return RedirectToAction("Index");
                }

                var (users, error) = await userServices.GetAllUsers();

                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.ErrorMessage = error;
                    return View("Index", new List<UserPublicProfile>());
                }

                var filteredUsers = users?.Where(u =>
                    u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(u.Name) && u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList() ?? new List<UserPublicProfile>();

                ViewBag.SearchTerm = searchTerm;
                ViewBag.ResultCount = filteredUsers.Count;

                return View("Index", filteredUsers);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred during search.";
                return View("Index", new List<UserPublicProfile>());
            }
        }

        // GET: User/MyProfile - Show current user's profile
        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            try
            {
                var userId = await GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                return RedirectToAction("PrivateProfile", new { id = userId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading your profile.";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult UserNavBar()
        {
            return View();
        }

        // Helper method to get current user ID from claims
        private async Task<string?> GetCurrentUserId()
        {
            return User.FindFirst("Id")?.Value ??
                   User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ??
                   User.FindFirst("sub")?.Value; 
        }



    }
}