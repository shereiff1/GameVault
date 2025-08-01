using System.ComponentModel.DataAnnotations;

namespace GameVault.BLL.ModelVM
{
    public class CompanyVM
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [MaxLength(200, ErrorMessage = "Company Name cannot exceed 200 characters.")]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Company Info cannot exceed 500 characters.")]
        public string? CompanyInfo { get; set; }

        [Required(ErrorMessage = "Created By is required.")]
        [MaxLength(100, ErrorMessage = "Created By cannot exceed 100 characters.")]
        public string CreatedBy { get; set; } = string.Empty;

    }
}
