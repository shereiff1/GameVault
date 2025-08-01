

using System.ComponentModel.DataAnnotations;

namespace GameVault.BLL.ModelVM.User
{
    public class UserLogin
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
