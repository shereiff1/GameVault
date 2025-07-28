
using System.ComponentModel.DataAnnotations.Schema;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Entites
{
    public class Game
    {
        public int GameId { get; private set; }
        public string Title { get; private set; }
        public DateTime? CreatedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string CreatedBy { get; private set; }

        public List<Category>? Categorys { get; set; } = new List<Category>();
        public List<Review>? Reviews { get; set; } = new List<Review>();
        public List<User>? Users { get; set; } = new List<User>();

        [ForeignKey("Company")]
        public int? CompanyId { get; private set; }
        public Company? Company { get; private set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; private set; }
        public Category? Category { get; private set; }
    }
}
