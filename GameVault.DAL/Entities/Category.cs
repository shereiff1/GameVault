

using GameVault.DAL.Entites;

namespace GameVault.DAL.Entities
{
    public class Category
    {
        public int CategoryID;
        public string CategoryName;
        public string Description;
        public DateTime? CreatedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public string CreatedBy { get; private set; }

        public List<Game>? Games { get; set; } = new List<Game>();
    }
}
