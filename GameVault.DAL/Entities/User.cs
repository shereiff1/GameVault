
using System.ComponentModel.DataAnnotations;

using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameVault.DAL.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(150)]
        public string? Name { get; private set; }
        public DateTime CreatedOn { get; private set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; private set; }
        public bool? IsDeleted { get; private set; } = false;
        public DateTime? DeletedOn { get; private set; }
        public string? ProfilePicture { get; private set; }
        public List<Game>? Library {get; private set;} = new List<Game>();

        //public List<User>? Friends { get; private set; } = new List<User>();

        public User() {
        }

        //public User(string Email, string Username, string Password) : base()
        //{
        //    this.Email = Email;
        //    this.UserName = Username;
        //    this.PasswordHash = Password;
            
        //}

        public void UpdateProfile(string? Username, string? Name, string? ImagePath)
        {
            if(Name != null)
                this.Name = Name;
            if(UserName != null)
                this.UserName = Username;
            this.ModifiedOn = DateTime.Now;
            if(ImagePath != null)
                this.ProfilePicture = ImagePath;
        }
        public void DeleteUser()
        {
            this.IsDeleted = true;
            this.DeletedOn = DateTime.Now;
        }

        public void AddGameToLibrary(Game game)
        {
            if (game == null) 
                throw new ArgumentNullException(nameof(game));
            if (Library == null) 
                Library = new List<Game>();
            Library.Add(game);
        }   

        public void RemoveGameFromLibrary(Game game)
        {
            if (game == null) 
                throw new ArgumentNullException(nameof(game));
            if (Library == null) 
                return;
            Library.Remove(game);
        }
        //public void AddFriend(User friend)
        //{
        //    if (friend == null) 
        //        throw new ArgumentNullException(nameof(friend));
        //    if (Friends == null) 
        //        Friends = new List<User>();
        //    Friends.Add(friend);
        //}
        //public void RemoveFriend(User friend)
        //{
        //    if (friend == null)
        //        throw new ArgumentNullException(nameof(friend));
        //    if (Friends == null)
        //        return;
        //    Friends.Remove(friend);
        //}

    }
}
