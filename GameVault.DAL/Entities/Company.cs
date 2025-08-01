using GameVault.DAL.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameVault.DAL.Entities
{
    public class Company
    {
        [Key]
        public int CompanyId { get; private set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; private set; }

        [MaxLength(500)]
        public string? CompanyInfo { get; private set; }

        public DateTime CreatedOn { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool IsDeleted { get; private set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; private set; }

        public virtual ICollection<Game> Games { get; private set; } = new List<Game>();
        public virtual Inventory Inventory { get; private set; }

        private Company() { }

        public Company(string companyName, string companyInfo, string createdBy)
        {
            CompanyName = companyName;
            CompanyInfo = companyInfo;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
        }

        public void UpdateCompanyInfo(string companyInfo)
        {
            CompanyInfo = companyInfo;
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
    }
}
