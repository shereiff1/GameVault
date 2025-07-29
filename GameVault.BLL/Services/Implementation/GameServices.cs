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

        public bool Add(GameDTO gameDto)
        {
            try
            {
                var game = _mapper.Map<Game>(gameDto);
                return _gameRepo.Add(game);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public (bool, List<GameDTO>?) GetAll(bool includeDeleted = false)
        {
            try
            {
                var (success, games) = _gameRepo.GetAll(includeDeleted);
                return (success, games != null ? _mapper.Map<List<GameDTO>>(games) : null);
            }
            catch (Exception ex)
            {

                return (false, null);
            }
        }

        public (bool, GameDTO?) GetById(int gameId)
        {
            try
            {
                var (success, game) = _gameRepo.GetById(gameId);
                return (success, game != null ? _mapper.Map<GameDTO>(game) : null);
            }
            catch (Exception ex)
            {

                return (false, null);
            }
        }

        public bool Update(GameDTO gameDto)
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

        public (bool, List<GameDTO>) GetByCompany(int companyId)
        {
            try
            {
                var (success, games) = _gameRepo.GetByCompany(companyId);
                return (success, _mapper.Map<List<GameDTO>>(games));
            }
            catch (Exception ex)
            {

                return (false, new List<GameDTO>());
            }
        }
    }
}
