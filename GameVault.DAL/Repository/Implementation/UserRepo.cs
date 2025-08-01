
using GameVault.DAL.Database;
using GameVault.DAL.Entites;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.DAL.Repository.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext db;

        public UserRepo(ApplicationDbContext db) 
        {
            this.db = db;
        }
        public bool AddUser(User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<User>?GetAll()
        {
            try
            {
                var users = db.Users.ToList();
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User GetUserById(string id)
        {
            try
            {
               var user = db.Users.Where(a => a.Id == id).FirstOrDefault();
                if (user == null)
                    return null;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool Update(User user)
        {
            try
            {
                db.Users.Update(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AddGameToLibrary(User user, Game game)
        {
            try
            {
                user.AddGameToLibrary(game);
                db.Users.Update(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveGameFromLibrary(User user, Game game)
        {
            try
            {
               user.RemoveGameFromLibrary(game);
                db.Users.Update(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //public bool AddFriend(User user, User friend)
        //{
        //    try
        //    {
        //       user.AddFriend(friend);
        //        db.Users.Update(user);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        //public bool RemoveFriend(User user, User friend)
        //{
        //    try
        //    {
        //       user.RemoveFriend(friend);
        //        db.Users.Update(user);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

    }
}
