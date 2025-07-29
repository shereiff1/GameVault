

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
            var game = _mapper.Map<Game>(gameDto);
            return _gameRepo.Add(game);
        }

        public (bool, List<GameDTO>?) GetAll(bool includeDeleted = false)
        {
            var (success, games) = _gameRepo.GetAll(includeDeleted);
            return (success, games != null ? _mapper.Map<List<GameDTO>>(games) : null);
        }

        public (bool, GameDTO?) GetById(int gameId)
        {
            var (success, game) = _gameRepo.GetById(gameId);
            return (success, game != null ? _mapper.Map<GameDTO>(game) : null);
        }

        public bool Update(GameDTO gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            return _gameRepo.Update(game);
        }

        public bool Delete(int gameId)
        {
            return _gameRepo.Delete(gameId);
        }

        public (bool, List<GameDTO>) GetByCompany(int companyId)
        {
            var (success, games) = _gameRepo.GetByCompany(companyId);
            return (success, _mapper.Map<List<GameDTO>>(games));
        }
    }
}
