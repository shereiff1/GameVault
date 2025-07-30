using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entites;
using GameVault.DAL.Repository.Abstraction;

namespace GameVault.BLL.Services.Implementation
{
    public class GameServices : IGameServices
    {
        private readonly IGameRepo _gameRepo;
        private readonly IMapper _mapper;

        public GameServices(IGameRepo gameRepo, IMapper mapper)
        {
            _gameRepo = gameRepo;
            _mapper = mapper;
        }

        public bool Add(int companyId, GameVM gameDto)
        {
            try
            {
                var game = _mapper.Map<Game>(gameDto);
                return _gameRepo.Add(game, companyId);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public (bool, List<GameVM>?) GetAll(bool includeDeleted = false)
        {
            try
            {
                var (success, games) = _gameRepo.GetAll(includeDeleted);
                return (success, games != null ? _mapper.Map<List<GameVM>>(games) : null);
            }
            catch (Exception ex)
            {

                return (false, null);
            }
        }

        public (bool, GameVM?) GetById(int gameId)
        {
            try
            {
                var (success, game) = _gameRepo.GetById(gameId);
                return (success, game != null ? _mapper.Map<GameVM>(game) : null);
            }
            catch (Exception ex)
            {

                return (false, null);
            }
        }

        public bool Update(GameVM gameDto)
        {
            try
            {
                var game = _mapper.Map<Game>(gameDto);
                return _gameRepo.Update(game);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

                return false;
            }
        }

        public bool Delete(int gameId)
        {
            try
            {
                return _gameRepo.Delete(gameId);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public (bool, List<GameVM>) GetByCompany(int companyId)
        {
            try
            {
                var (success, games) = _gameRepo.GetByCompany(companyId);
                return (success, _mapper.Map<List<GameVM>>(games));
            }
            catch (Exception ex)
            {

                return (false, new List<GameVM>());
            }
        }
    }
}
