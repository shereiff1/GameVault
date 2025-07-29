using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameVault.DAL.Entities;

namespace GameVault.DAL.Entites
{
    public class Game
    {
        [Key]
        public int GameId { get; private set; }


        [Required]
        [MaxLength(200)]
        public string Title { get;  set; }

        public DateTime CreatedOn { get;  set; }
        public DateTime? ModifiedOn { get;  set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get;  set; }

        public bool IsDeleted { get;  set; }

        [Required]
        [ForeignKey("Company")]
        public int CompanyId { get;  set; }

        public virtual Company Company { get;  set; }
        public virtual List<Review>? Reviews { get; set; } = new List<Review>();

        public virtual List<Category>? Categories { get; set; } = new List<Category>();

        public virtual List<User>? Users { get; set; } = new List<User>();

        private Game() { } 

        public Game(string title, int companyId, string createdBy)
        {
            Title = title;
            CompanyId = companyId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
            ModifiedOn = DateTime.Now;
        }

        public void UpdateCompany(int companyId)
        {
            CompanyId = companyId;
            ModifiedOn = DateTime.UtcNow;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            ModifiedOn = DateTime.Now;
        }

        public void Restore()
        {
            IsDeleted = false;
            ModifiedOn = DateTime.Now;
        }
    }
}