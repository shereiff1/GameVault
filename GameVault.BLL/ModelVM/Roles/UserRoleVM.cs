

using System.ComponentModel.DataAnnotations;

namespace GameVault.BLL.ModelVM.Roles
{
   public class UserRoleVM
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfilePicture { get;  set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public bool IsSelected { get; set; }
    }
}
