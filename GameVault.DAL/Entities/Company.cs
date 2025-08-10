using System.ComponentModel.DataAnnotations;

namespace GameVault.DAL.Entities
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        [MaxLength(500)]
        public string? CompanyInfo { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

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
