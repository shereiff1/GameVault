
using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IGameServices
    {
        bool Add(GameDTO gameDto);
        (bool, List<GameDTO>?) GetAll(bool includeDeleted = false);
        (bool, GameDTO?) GetById(int gameId);
        bool Update(GameDTO gameDto);
        bool Delete(int gameId);
        (bool, List<GameDTO>) GetByCompany(int companyId);
    }
}
