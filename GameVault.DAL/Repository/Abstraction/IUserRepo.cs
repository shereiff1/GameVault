

using GameVault.DAL.Entites;
using GameVault.DAL.Entities;

using GameVault.DAL.Entites;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IUserRepo
    {
        bool AddUser(User user);
        List<User>? GetAll();
        bool Update(User user);

        User GetUserById(string id);
        bool AddGameToLibrary(User user, Game game);
        bool RemoveGameFromLibrary(User user, Game game);
        //bool AddFriend(User user, User friend);
        //bool RemoveFriend(User user, User friend);

    }
}
