
using GameVault.DAL.Entities;

namespace GameVault.BLL.ModelVM.User
{
    public class UserPrivateProfile
    {
        public string Id { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? UserName { get; set; }

        public DateTime CreatedOn { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string? ProfilePicture { get; private set; }
        public List<Game>? Library { get; private set; } = new List<Game>();


        //public List<User>? Friends { get; private set; } = new List<User>();


    }
}
