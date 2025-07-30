
namespace GameVault.BLL.ModelVM
{
    public class InventoryItemVM
    {
        // required attributes 
        public int InventoryItemId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
