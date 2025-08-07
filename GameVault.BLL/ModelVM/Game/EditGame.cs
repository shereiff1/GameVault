using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameVault.BLL.ModelVM.Game
{
    public class EditGame
    {
        public int GameId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        public IFormFile formFile { get; set; }
        public string? ImagePath { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
