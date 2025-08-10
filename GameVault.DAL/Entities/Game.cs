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
        public string Title { get; private set; }
        public string ImagePath { get; private set; }

        public DateTime CreatedOn { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string Description { get; private set; }
        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; private set; }
        public bool IsDeleted { get; private set; }
        [ForeignKey("Company")]
        public int CompanyId { get; private set; }
        public virtual Company Company { get; private set; }
        public virtual List<Review>? Reviews { get; private set; } = new();
        public virtual List<Category>? Categories { get; private set; } = new();
        public virtual List<User>? Users { get; private set; } = new();
        private Game() { }
        public Game(string title, int companyId, string createdBy, string description, string imagePath)
        {
            Title = title;
            CompanyId = companyId;
            CreatedBy = createdBy;
            ImagePath = imagePath;
            CreatedOn = DateTime.Now;
            Description = description;
            IsDeleted = false;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
            ModifiedOn = DateTime.Now;
        }
        public void UpdatePhoto(string imagePath)
        {
            ImagePath = imagePath;
            ModifiedOn = DateTime.Now;
        }
        public void UpdateCompany(int companyId)
        {
            CompanyId = companyId;
            ModifiedOn = DateTime.Now;
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

        public void Update(string title, string description)
        {
            this.Title = title;
            this.Description = description;
        }
    }
}
