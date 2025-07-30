

using System.ComponentModel.DataAnnotations.Schema;
using GameVault.DAL.Entites;

namespace GameVault.DAL.Entities
{
    public class Review
    {
        public Review(string comment, float rating, int userId, string createdBy, int gameId)
        {
            Comment = comment;
            Rating = rating;
            UserId = userId;
            CreatedOn = DateTime.Now;
            CreatedBy = createdBy;
            GameId = gameId;
            IsDeleted = false;
        }

        public int ReviewId { get;  set; }
        public string Comment { get;  set; }
        public float Rating { get;  set; }
        [ForeignKey("User")]
        public int UserId { get;  set; }
        public DateTime? CreatedOn { get;  set; }
        public bool IsDeleted { get;  set; }
        public DateTime? ModifiedOn { get;  set; }
        public string CreatedBy { get;  set; }
        [ForeignKey("Game")]
        public int GameId { get; set; }



    }
}
