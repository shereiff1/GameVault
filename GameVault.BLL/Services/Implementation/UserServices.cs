
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

namespace GameVault.BLL.Services.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepo userrepo;
        private readonly IGameRepo gamerepo;
        private readonly IMapper mapper;

        public UserServices(IUserRepo repo, IMapper mapper,IGameRepo gamerepo)
        {
            this.userrepo = repo;
            this.mapper = mapper;
            this.gamerepo = gamerepo;
        }


        public async Task<(bool, string?)> UpdateUserProfile(UpdateUserProfile user)
        {
            try
            {
                if (user == null)
                    return (false,"Ivalid Update, please try again");
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

        public async Task<(List<UserPrivateProfile>?, string)> GetAllPrivateUsers()
        {
            try
            {
                var users =await userrepo.GetAll();
                if (users == null)
                    return (null, "There are no users yet");
                var map = mapper.Map<List<UserPrivateProfile>>(users);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(List<UserPublicProfile>?, string)> GetAllUsers()
        {
            try
            {
                var users = await userrepo.GetAll();
                if (users == null)
                    return (null, "There are no users yet");
                var map = mapper.Map<List<UserPublicProfile>>(users);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(UserPublicProfile?, string?)> GetPublicProfile(string id)
        {
            try
            {
                var user =await userrepo.GetUserById(id);
                if (user == null)
                    return (null, "Error, User not found");
                var map = mapper.Map<UserPublicProfile>(user);
                return (map, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(UserPrivateProfile?, string?)> GetPrivateProfile(string id)
        {
            try
            {
                var user =await userrepo.GetUserById(id);
                if (user == null)
                    return (null, "Error, User not found");
                var map = mapper.Map<UserPrivateProfile>(user);
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

        public async Task<(bool,string?)> AddGameToLibrary(string userId, int gameId)
        {
            try
            {
                var user = await userrepo.GetUserById(userId);
                var (result, game) = await gamerepo.GetByIdAsync(gameId);

                if (user == null || result == false)
                    return (false,"Error, Invalid user or game");

                if (userrepo.IsGameInUserLibrary(userId,game).Result)
                {
                    return (false, "Game Already in your Library");
                }

                await userrepo.AddGameToLibrary(user, game);
                return (true,null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool,string?)> RemoveGameFromLibrary(string userId, int gameId)
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
                return (true,null);
            }
            catch (Exception ex)
            {
                return (false,ex.Message);
            }
        }

        public async Task<(List<ModelVM.GameVM>?,string?)> GetUserLibrary(string userId)
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

                return(null,ex.Message);
            }
           

        }

    }
}
