

using System.ComponentModel.DataAnnotations.Schema;
using GameVault.DAL.Entites;

namespace GameVault.DAL.Entities
{
    public class Review
    {
        public int ReviewId { get; private set; }
        public string Comment { get; private set; }
        public float Rating { get; private set; }
        [ForeignKey("User")]
        public int UserId { get; private set; }
        public DateTime? CreatedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string CreatedBy { get; private set; }
        [ForeignKey("Game")]
        public int GameId { get; set; }

    }
}
