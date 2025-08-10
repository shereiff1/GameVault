using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameVault.BLL.ModelVM.User;

namespace GameVault.BLL.ModelVM.Roles
{
    public class EditRoleVM
    {
        public EditRoleVM()
        {
            Users = new List<UserPublicProfile>();
        }

        public string Id { get; set; }

        public List<UserPublicProfile> Users { get; set; }
    }
}
