using System.ComponentModel.DataAnnotations;
namespace GameVault.DAL.Entities
{
    public class Category
    {
        [Key]
        public int Category_Id { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Category_Name { get; private set; }

        [MaxLength(500)]
        public string? Description { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? ModifiedOn { get; private set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; private set; }
        public virtual List<Game>? Games { get; set; } = new List<Game>();
    public Category()
        {
            CreatedOn = DateTime.Now;
            IsDeleted = false;
            ModifiedOn = DateTime.Now;
        }
        public Category( string name, string discription, string createdBy ="Admin")
        {
            Category_Name = name;
            Description = discription;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
            ModifiedOn = DateTime.Now;
            CreatedBy = createdBy;
        }
        public Category(int id, string name, string discription, string createdBy = "Admin")
        {
            Category_Id = id;
            Category_Name = name;
            Description = discription;
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
        public void Update(int id, string name, string discription)
        {
            Category_Id = id;
            Category_Name = name;
            Description = discription;
            ModifiedOn = DateTime.Now;
        }

    }
}
