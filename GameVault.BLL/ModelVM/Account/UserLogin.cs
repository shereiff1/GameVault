

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace GameVault.BLL.ModelVM.Account
{
    public class UserLogin
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
