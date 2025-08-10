
using GameVault.DAL.Entites;

namespace GameVault.BLL.ModelVM.User
{
    public class UserPrivateProfile
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? UserName { get; set; }

        public DateTime CreatedOn { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string? ProfilePicture { get; private set; }
        public List<GameVM>? Library { get; private set; } = new List<GameVM>();


        //public List<User>? Friends { get; private set; } = new List<User>();


    }
}
