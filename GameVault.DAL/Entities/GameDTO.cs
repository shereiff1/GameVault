

namespace GameVault.DAL.Entities
{
    public class GameDTO
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Category>? Categories { get; set; }
        public float Rating { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
    }
}
