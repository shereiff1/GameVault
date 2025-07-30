

using GameVault.DAL.Entites;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IGameRepo
    {
        bool Add(Game game,int companyId);
        (bool,List<Game>?) GetAll(bool includeDeleted = false);
        (bool,Game?) GetById(int gameId);
        bool Update(Game game);
        bool Delete(int gameId);
        (bool,List<Game>) GetByCompany(int companyId);
    }
}
