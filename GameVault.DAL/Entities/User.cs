
namespace GameVault.DAL.Entities
{
    public class User
    {
        public int User_ID {get; private set;}
        public List<Game>? Games {get; set;} = new List<Game>();
    }
}
