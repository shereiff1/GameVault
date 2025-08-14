
using System.Reflection.Emit;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.User;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IUserServices
    {
        //public (bool,string?) AddUser(UserSignUp user);
        public Task<(List<UserPublicInfo>?,string)> GetAllUsers();
        public Task<(bool,string?)> UpdateUserProfile(UpdateUserProfile user);
        public Task<(List<UserProfile>?, string)> GetAllUsersProfiles();

        public Task<(UserProfile?, string?)> GetProfile(string id);

        public Task<(UserPublicInfo?, string?)> GetPublicInfo(string id);
        public Task<(UpdateUserProfile?, string?)> GetUserInfo(string id);

        public Task<(bool,string?)> AddGameToLibrary(string userId, int gameId);
        public Task<(bool,string?)> RemoveGameFromLibrary(string userId, int gameId);

        public Task<(List<GameVM>?,string?)> GetUserLibrary(string userId);

        public Task<(List<UserPublicInfo>?, string)> GetAllAdmins();

        public Task<(bool,string?)> DeleteUser(string userId);

        public Task<(UserPublicInfo, string?)> GetUserById(string id);

    }
}
