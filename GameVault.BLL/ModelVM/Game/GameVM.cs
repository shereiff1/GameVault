
using Microsoft.AspNetCore.Http;

namespace GameVault.BLL.ModelVM
{
    public class GameVM
    {
        // the ctor string title int companyId, string createdBy
        public int GameId { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public IFormFile formFile { get; set; }
        //public string ImagePath { get; set; }
        public decimal Price { get; set; }

    }
}
