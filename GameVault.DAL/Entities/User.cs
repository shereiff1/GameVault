
using GameVault.DAL.Entites;

namespace GameVault.DAL.Entities
{
    public class User
    {
        public int UserId {get; private set;}
        public List<Game>? Games {get; set;} = new List<Game>();
    }
}
