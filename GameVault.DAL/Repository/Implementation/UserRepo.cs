
using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace GameVault.DAL.Repository.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext db;


        public UserRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<bool> AddUser(User user)
        {
            try
            {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<User>?> GetAll()
        {
            try
            {
                var users = await db.Users.Where(a => a.IsDeleted == false).ToListAsync();
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User?> GetUserById(string id)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(a => a.Id == id);

                if (user == null)
                    return null;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> Update(User user)
        {
            try
            {
                 db.Users.Update(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> AddGameToLibrary(User user, Game game)
        {
            try
            {
                user.AddGameToLibrary(game);
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> RemoveGameFromLibrary(User user, Game game)
        {
            try
            {
                 user.RemoveGameFromLibrary(game);
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> IsUserInRole(User user,string rolename)
        {
            try
            {
                var inRole = await db.UserRoles
                 .AnyAsync(ur => ur.UserId == user.Id &&
                  ur.RoleId == db.Roles
                      .Where(r => r.Name == rolename)
                      .Select(r => r.Id)
                      .FirstOrDefault());
                return inRole;
            }
            catch (Exception)
            {
                throw;
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
