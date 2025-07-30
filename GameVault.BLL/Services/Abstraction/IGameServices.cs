
using GameVault.BLL.ModelVM;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IGameServices
    {
        bool Add(int companyId, GameVM gameDto);
        (bool, List<GameVM>?) GetAll(bool includeDeleted = false);
        (bool, GameVM?) GetById(int gameId);
        bool Update(GameVM gameDto);
        bool Delete(int gameId);
        (bool, List<GameVM>) GetByCompany(int companyId);
    }
}
