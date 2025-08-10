

using Microsoft.AspNetCore.Identity;

namespace GameVault.BLL.ModelVM.Account
{
    public class ExternalLoginResult
    {
        public bool Success { get; set; }
        public bool RequiresUserCreation { get; set; }
        public  ExternalLoginInfo ExternalLoginInfo { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class ExternalLoginCreationResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
