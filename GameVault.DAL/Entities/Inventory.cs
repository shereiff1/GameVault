
﻿using GameVault.DAL.Entites;

using GameVault.DAL.Entites;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameVault.DAL.Entities
{
    public class Inventory
    {
        [Key, ForeignKey(nameof(Company))]
        public int CompanyId { get; private set; }

        public virtual Company Company { get; private set; }

        public virtual ICollection<InventoryItem> Items { get; private set; } = new List<InventoryItem>();

        private Inventory() { }

        public Inventory(int companyId)
        {
            CompanyId = companyId;
        }

        public void AddOrUpdateItem(int gameId, int quantity, decimal price)
        {
            var existing = Items.FirstOrDefault(i => i.GameId == gameId);

            if (existing is null)
            {
                Items.Add(new InventoryItem(this, gameId, quantity, price));
            }
            else
            {
                existing.SetQuantity(quantity);
                existing.SetPrice(price);
            }
        }
    }
}
