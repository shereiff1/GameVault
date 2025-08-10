
using GameVault.DAL.Entites;
using Microsoft.AspNetCore.Http;

namespace GameVault.BLL.ModelVM.User
{
    public class UpdateUserProfile
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public IFormFile? ImageFile { get;  set; }

        public string? ProfilePicture { get; set; }
    }
}
