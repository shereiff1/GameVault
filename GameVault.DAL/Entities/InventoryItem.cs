using GameVault.DAL.Entites;
using GameVault.DAL.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class InventoryItem
{
    [Key]
    public int InventoryItemId { get; private set; }

    [Required]
    public int CompanyId { get; private set; }
    public virtual Company Company { get; private set; }

    [Required]
    public int GameId { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    [Required]
    public decimal Price { get; private set; }

    public int Quantity { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    [NotMapped]
    public bool CanBeSold => Price > 0;

    public virtual Game Game { get; private set; }


    private InventoryItem() { }

    public InventoryItem(Company company, int gameId, decimal price)
    {
        Company = company ?? throw new ArgumentNullException(nameof(company));
        CompanyId = company.CompanyId;
        GameId = gameId;
        SetPrice(price);

        CreatedOn = DateTime.UtcNow;
        Quantity = 0;
    }

    public void SetPrice(decimal price)
    {
        Price = price;
        UpdatedOn = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
        UpdatedOn = DateTime.UtcNow;
    }
}
