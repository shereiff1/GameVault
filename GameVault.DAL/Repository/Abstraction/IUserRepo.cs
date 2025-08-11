

using GameVault.DAL.Entities;


namespace GameVault.DAL.Repository.Abstraction
{
    public interface IUserRepo
    {
        Task<bool> AddUser(User user);
        Task<List<User>?> GetAll();
        Task<bool> Update(User user);

        Task<User?> GetUserById(string id);
        Task<bool> AddGameToLibrary(User user, Game game);
        Task<bool> RemoveGameFromLibrary(User user, Game game);

        public Task<bool> IsGameInUserLibrary(string userId, Game game);

        public Task<List<Game>?> GetUserLibrary(string userId);

        //bool AddFriend(User user, User friend);
        //bool RemoveFriend(User user, User friend);

        public Task<bool> IsUserInRole(User user, string rolename);

    }
}
