

namespace GameVault.DAL.Entities
{
    public class GameDTO
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public List<string>? Reviews { get; set; }
        public List<string>? Categories { get; set; }
        public float Rating { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
    }
}
