
using System.Threading.Tasks;
using _3TierArch.BLL.Helper;
using AutoMapper;

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
        private readonly IUserRepo repo;
        private readonly IMapper mapper;

        public UserServices(IUserRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }


        //public (bool, string?) AddUser(UserSignUp user)
        //{
        //    try
        //    {
        //        if(user == null) 
        //            return (false, "Invalid SingUp info");
        //        var User = new User(user.Email,user.Username,user.Password);
        //        return (repo.AddUser(User), null);

        //    }
        //    catch (Exception ex)
        //    {

        //        return (false, ex.Message);
        //    }
        //}

        public async Task<(bool, string?)> UpdateUserProfile(UpdateUserProfile user)
        {
            try
            {
                if (user == null)
                    return (false,"Ivalid Update, please try again");
                user.ProfilePicture = Upload.UploadFile("Images", user.ImageFile);
                var User = await repo.GetUserById(user.Id);
                User.UpdateProfile(user.UserName, user.Name, user.ProfilePicture);
                return (await repo.Update(User), null);
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
                var users =await repo.GetAll();
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

        async Task<(List<UserPublicProfile>?, string)> IUserServices.GetAllUsers()
        {
            try
            {
                var users = await repo.GetAll();
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
                var user =await repo.GetUserById(id);
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
                var user =await repo.GetUserById(id);
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
                var user = await repo.GetUserById(id);
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


    }
}
