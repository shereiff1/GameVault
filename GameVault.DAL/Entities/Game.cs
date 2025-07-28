
namespace GameVault.DAL.Entites
{
    public class Game
    {
        public int Game_Code { get; private set; }
        public string title { get; private set; }
        public List<Category>? Categorys { get; set; } = new List<Category>();
        public List<Review>? Reviews { get; set; } = new List<Review>();
        public List<User>? Users { get; set; } = new List<User>();

        [ForeignKey("Company")]
        public int? Company_ID { get; private set; };
        public Company? Company { get; private set; }
        [ForeignKey("Category")]
        public int? Category_ID { get; private set; };
        public Category? Category { get; private set; }

    }
}
