
namespace GameVault.DAL.Entities
{
    public class Company
    {
        public int Comp_ID;
        public string Comp_Name;
        public string Comp_Info;
         public DateTime? CreatedOn { get; private set; }
         public bool IsDeleted { get; private set; }
         public DateTime? ModifiedOn { get; private set; }
         public string CreatedBy { get; private set; }
        
        public List<Game>? Games { get; set; } = new List<Game>();


    }
}
