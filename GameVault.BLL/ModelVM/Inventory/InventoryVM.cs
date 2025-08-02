

namespace GameVault.BLL.ModelVM
{
    public class InventoryVM
    {

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<InventoryItemVM> Items { get; set; }
    }
}
