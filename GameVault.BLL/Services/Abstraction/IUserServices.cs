
using GameVault.BLL.ModelVM.User;
using GameVault.DAL.Entites;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IUserServices
    {
        public (bool,string?) AddUser(UserSignUp user);
        public (List<UserPublicProfile>?,string) GetAllUsers();
        public (bool,string?) UpdateUserProfile(UpdateUserProfile user);
        public (bool,string?) UserLogin(UserLogin user);
        public (List<UserPrivateProfile>?, string) GetAllPrivateUsers();

    }
}
