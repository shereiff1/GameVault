
using AutoMapper;

using GameVault.BLL.ModelVM.User;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using GameVault.DAL.Repository.Implementation;

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


        public (bool, string?) AddUser(UserSignUp user)
        {
            try
            {
                if(user == null) 
                    return (false, "Invalid SingUp info");
                var User = new User(user.Email,user.Username,user.Password);
                return (repo.AddUser(User), null);

            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }

        public (bool, string?) UpdateUserProfile(UpdateUserProfile user)
        {
            try
            {
                if (user == null)
                    return (false,"Ivalid Update, please try again");
                var User = repo.GetUserById(user.Id);
                User.UpdateProfile(user.UserName, user.Name, user.ProfilePicture);
                return (repo.Update(User), null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<UserPrivateProfile>?, string) GetAllPrivateUsers()
        {
            try
            {
                var users = repo.GetAll();
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

      
        public (bool, string?) UserLogin(UserLogin user)
        {
            try
            {
                // fix this here too
                return (true, "Login failed");
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }

        (List<UserPublicProfile>?, string) IUserServices.GetAllUsers()
        {
            try
            {
                var users = repo.GetAll();
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
    }
}
