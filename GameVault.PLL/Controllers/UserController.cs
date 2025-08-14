using GameVault.BLL.ModelVM.User;
using GameVault.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameVault.BLL.ModelVM.Game;

namespace GameVault.PLL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices userServices;

        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        
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

        public async Task<IActionResult> AllAdmins()
        {
            try
            {
                var (users, error) = await userServices.GetAllAdmins();

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

        [Authorize]
        public async Task<IActionResult> UserProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("User ID is required.");
            }

            try
            {
                var (user, error) = await userServices.GetProfile(id);

                if (!string.IsNullOrEmpty(error))
                {
                    return View(error);
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading user profile.";
                return View();
            }
        }
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {

            try
            {
                var (user, error) = await userServices.GetProfile(id);

                if (!string.IsNullOrEmpty(error))
                {
                    return View();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while getting the user.";
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var (success, error) = await userServices.DeleteUser(id);

                if (!success)
                {
                    ViewBag.Message = error;
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while getting the user.";
                return View();
            }
        }
        [Authorize]
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return RedirectToAction("Index","Home");
                }

                var (users, error) = await userServices.GetAllUsers();

                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.ErrorMessage = error;
                    return View("Index", new List<UserPublicInfo>());
                }

                var filteredUsers = users?.Where(u =>
                    u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(u.Name) && u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList() ?? new List<UserPublicInfo>();

                ViewBag.SearchTerm = searchTerm;
                ViewBag.ResultCount = filteredUsers.Count;

                return View("SearchResult", filteredUsers);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred during search.";
                return View("SearchResult", new List<UserPublicInfo>());
            }
        }

        public async Task<IActionResult> SearchResult(List<UserPublicInfo> Result)
        {
            try
            {
                if (Result == null || !Result.Any())
                {
                    return RedirectToAction("Index","Home");
                }

                ViewBag.ResultCount = Result.Count;

                return View(Result);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while displaying search results.";
                return RedirectToAction("Index","Home");
            }
        }

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

                return RedirectToAction("UserProfile", new { id = userId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading your profile.";
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]

        [HttpPost]
        public async Task<IActionResult> AddGameToLibrary(int gameId)
        {
            var userId = GetCurrentUserId().Result;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }


            var (success, error) = await userServices.AddGameToLibrary(userId, gameId);

            if (!success)
            {
                ViewBag.ErrorMessage = error ?? "Failed to Add the Game";
                return View();
            }

            return RedirectToAction("Library", "User");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveGameFromLibrary(int gameId)
        {
            var userId = GetCurrentUserId().Result;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }


            var (success, error) = await userServices.RemoveGameFromLibrary(userId, gameId);

            if (!success)
            {
                ViewBag.ErrorMessage = error ?? "Failed to remove the Game";
                return View();
            }

            return RedirectToAction("Library", "User");
        }

        [Authorize]
        public async Task<IActionResult> UserLibrary(string id)
        {
            try
            {

                var (user,Error) = await userServices.GetUserById(id);
                if (!string.IsNullOrEmpty(Error))
                {
                    return RedirectToAction("Index", "Home");
                }

                var (Library,error) = await userServices.GetUserLibrary(user.Id);

                if(error != null)
                {
                    ViewBag.ErrorMessage = error ?? "Failed to load your games library";
                }
                ViewBag.UserId = user.Id;
                return View(Library);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading your Library.";
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public IActionResult UserNavBar(int id)
        {

            return View();
        }

        private async Task<string?> GetCurrentUserId()
        {
            return User.FindFirst("Id")?.Value ??
                   User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ??
                   User.FindFirst("sub")?.Value; 
        }

    }
}