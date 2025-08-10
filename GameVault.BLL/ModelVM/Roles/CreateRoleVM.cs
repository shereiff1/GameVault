using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameVault.BLL.ModelVM.Roles
{
    public class CreateRoleVM
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; } = DateTime.Now;
    }
}
