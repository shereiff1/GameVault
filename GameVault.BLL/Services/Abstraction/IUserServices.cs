
using System.Reflection.Emit;
using GameVault.BLL.ModelVM;
using GameVault.BLL.ModelVM.User;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IUserServices
    {
        //public (bool,string?) AddUser(UserSignUp user);
        public Task<(List<UserPublicProfile>?,string)> GetAllUsers();
        public Task<(bool,string?)> UpdateUserProfile(UpdateUserProfile user);
        public Task<(List<UserPrivateProfile>?, string)> GetAllPrivateUsers();

        public Task<(UserPrivateProfile?, string?)> GetPrivateProfile(string id);

        public Task<(UserPublicProfile?, string?)> GetPublicProfile(string id);
        public Task<(UpdateUserProfile?, string?)> GetUserInfo(string id);

        public Task<(bool,string?)> AddGameToLibrary(string userId, int gameId);
        public Task<(bool,string?)> RemoveGameFromLibrary(string userId, int gameId);

        public Task<(List<GameVM>?,string?)> GetUserLibrary(string userId);


    }
}
