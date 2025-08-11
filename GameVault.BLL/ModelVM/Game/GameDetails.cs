using GameVault.BLL.ModelVM.Category;
using GameVault.BLL.ModelVM.Review;
using GameVault.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace GameVault.BLL.ModelVM.Game
{
    public class GameDetails
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public  List<ReviewDTO>? Reviews { get;  set; } 
        public  List<CategoryDTO>? Categories { get;  set; } 
        public float Rating { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
    }
}
