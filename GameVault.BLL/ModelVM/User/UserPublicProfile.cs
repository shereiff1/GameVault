
namespace GameVault.BLL.ModelVM.User
{
    public class UserPublicProfile
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? ProfilePicture { get; private set; }
    }
}
