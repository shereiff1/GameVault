using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameVault.DAL.Entities
{
    public class InventoryItem
    {
        [Key]
        public int InventoryItemId { get; private set; }

        [Required]
        public int GameId { get; private set; }
        public virtual Game Game { get; private set; }

        [Required]
        public int CompanyId { get; private set; }
        public virtual Inventory Inventory { get; private set; }

        [Required]
        public int Quantity { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; private set; }

        public DateTime CreatedOn { get; private set; }
        public DateTime? UpdatedOn { get; private set; }

        private InventoryItem() { }

        public InventoryItem(Inventory inventory, int gameId, int quantity, decimal price)
        {
            Inventory = inventory;
            CompanyId = inventory.CompanyId;
            GameId = gameId;
            SetQuantity(quantity);
            SetPrice(price);
            CreatedOn = DateTime.Now;
        }

        public void SetQuantity(int qty)
        {
            Quantity = qty;
            UpdatedOn = DateTime.Now;
        }

        public void SetPrice(decimal price)
        {
            Price = price;
            UpdatedOn = DateTime.Now;
        }
    }
}
