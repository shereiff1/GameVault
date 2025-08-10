using GameVault.DAL.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameVault.DAL.Entities
{
    public class Review
    {
        [Key]
        public int Review_Id { get; private set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Player_Id must be a positive integer.")]
        public int Player_Id { get; private set; }

        public string Comment { get; private set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public float Rating { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? ModifiedOn { get; private set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; private set; }
        [ForeignKey("Game")]
        public int Game_Id { get; private set; }
        public virtual Game? Game { get; private set; }
        public Review()
        {
            CreatedOn = DateTime.Now;
            IsDeleted = false;
            ModifiedOn = DateTime.Now;
        }
        public Review(int player_id, string comment, float rating, int game_id, string createdBy = "Admin")
        {
            Player_Id = player_id;
            Comment = comment;
            Rating = rating;
            Game_Id = game_id;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
            ModifiedOn = DateTime.Now;
            CreatedBy = createdBy;
        }
        public void DELETE()
        {
            IsDeleted = true;
            ModifiedOn = DateTime.Now;
        }
        public void Update(int review_id, int player_id, string comment, float rating, string createdby)
        {
            Review_Id = review_id;
            Player_Id = player_id;
            Comment = comment;
            Rating = rating;
            CreatedBy = createdby;
            ModifiedOn = DateTime.Now;
        }
    }
}
