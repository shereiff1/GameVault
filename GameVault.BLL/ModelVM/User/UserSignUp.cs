using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace GameVault.BLL.ModelVM.User
{
    public class UserSignUp
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
