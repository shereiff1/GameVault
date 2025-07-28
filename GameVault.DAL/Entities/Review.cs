

namespace GameVault.DAL.Entities
{
    public class Review
    {
         public int Review_Id { get; private set; }
         public string Comment { get; private set; }
         public float Rating { get; private set; }
         public int Player_Id { get; private set; }
         public DateTime? CreatedOn { get; private set; }
         public bool IsDeleted { get; private set; }
         public DateTime? ModifiedOn { get; private set; }
         public string CreatedBy { get; private set; }
     public List<Game>? Games { get; set; } = new List<Game>();

    }
}
