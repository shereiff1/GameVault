
using System.Threading.Tasks;
using GameVault.BLL.Helpers.UploadFile;
using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.ModelVM.User;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using GameVault.DAL.Repository.Implementation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;

namespace GameVault.BLL.Services.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepo userrepo;
        private readonly IGameRepo gamerepo;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UserServices(IUserRepo repo, IMapper mapper, IGameRepo gamerepo, UserManager<User> userManager)
        {
            this.userrepo = repo;
            this.mapper = mapper;
            this.gamerepo = gamerepo;
            this.userManager = userManager;
        }


        public async Task<(bool, string?)> UpdateUserProfile(UpdateUserProfile user)
        {
            try
            {
                if (user == null)
                    return (false, "Ivalid Update, please try again");
                user.ProfilePicture = Upload.UploadFile("Images", user.ImageFile);
                var User = await userrepo.GetUserById(user.Id);
                User.UpdateProfile(user.UserName, user.Name, user.ProfilePicture);
                return (await userrepo.Update(User), null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(List<UserProfile>?, string)> GetAllUsersProfiles()
        {
            try
            {
                var users = await userrepo.GetAll();
                if (users == null)
                    return (null, "There are no users yet");
                var map = mapper.Map<List<UserProfile>>(users);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(List<UserPublicInfo>?, string)> GetAllUsers()
        {
            try
            {
                var users = await userrepo.GetAll();
                if (users == null)
                    return (null, "There are no users yet");
                var map = mapper.Map<List<UserPublicInfo>>(users);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(UserPublicInfo?, string?)> GetPublicInfo(string id)
        {
            try
            {
                var user = await userrepo.GetUserById(id);
                if (user == null)
                    return (null, "Error, User not found");
                var map = mapper.Map<UserPublicInfo>(user);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(UserProfile?, string?)> GetProfile(string id)
        {
            try
            {
                var user = await userrepo.GetUserById(id);
                if (user == null)
                    return (null, "Error, User not found");
                var map = mapper.Map<UserProfile>(user);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(UpdateUserProfile?, string?)> GetUserInfo(string id)
        {
            try
            {
                var user = await userrepo.GetUserById(id);
                if (user == null)
                    return (null, "Error, User not found");
                var map = mapper.Map<UpdateUserProfile>(user);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(bool, string?)> AddGameToLibrary(string userId, int gameId)
        {
            try
            {
                var user = await userrepo.GetUserById(userId);
                var (result, game) = await gamerepo.GetByIdAsync(gameId);

                if (user == null || result == false)
                    return (false, "Error, Invalid user or game");

                if (userrepo.IsGameInUserLibrary(userId, game).Result)
                {
                    return (false, "Game Already in your Library");
                }

                await userrepo.AddGameToLibrary(user, game);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string?)> RemoveGameFromLibrary(string userId, int gameId)
        {
            try
            {
                var user = await userrepo.GetUserById(userId);
                var (result, game) = await gamerepo.GetByIdAsync(gameId);

                if (user == null || result == false)
                    return (false, "Error, Invalid user or game");

                if (!userrepo.IsGameInUserLibrary(userId, game).Result)
                {
                    return (false, "Game is not your Library");
                }

                await userrepo.RemoveGameFromLibrary(user, game);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(List<ModelVM.GameVM>?, string?)> GetUserLibrary(string userId)
        {
            try
            {
                var library = await userrepo.GetUserLibrary(userId);
                if (library == null || library.Count == 0)
                {
                    return (null, "You don't have any games yet");
                }
                var map = mapper.Map<List<ModelVM.GameVM>>(library);
                return (map, null);
            }
            catch (Exception ex)
            {

                return (null, ex.Message);
            }


        }
        public async Task<(List<UserPublicInfo>?, string)> GetAllAdmins()
        {
            try
            {
                var users = await userManager.GetUsersInRoleAsync("Admin");
                if (users == null)
                    return (null, "There are no admins yet");
                var map = mapper.Map<List<UserPublicInfo>>(users);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(bool, string?)> DeleteUser(string userId)
        {
            try
            {
                var user = await userrepo.GetUserById(userId);

                if (user == null)
                    return (false, "Error,User Doesn't Exist");

                await userrepo.Delete(user);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(UserPublicInfo, string?)> GetUserById(string id)
        {
            try
            {
                var user = await userrepo.GetUserById(id);
                if (user == null)
                    return (null, "Error, User not found");
                var map = mapper.Map<UserPublicInfo>(user);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }

        }
    }
}
