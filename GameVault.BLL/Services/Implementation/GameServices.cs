using AutoMapper;
using GameVault.BLL.ModelVM;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
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

        public async Task<bool> AddAsync(int companyId, GameVM gameDto)
        {
            try
            {
                var game = _mapper.Map<Game>(gameDto);
                return await _gameRepo.AddAsync(game, companyId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<(bool, List<GameVM>?)> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var (success, games) = await _gameRepo.GetAllAsync(includeDeleted);
                return (success, games != null ? _mapper.Map<List<GameVM>>(games) : null);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<(bool, GameVM?)> GetByIdAsync(int gameId)
        {
            try
            {
                var (success, game) = await _gameRepo.GetByIdAsync(gameId);
                return (success, game != null ? _mapper.Map<GameVM>(game) : null);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<bool> UpdateAsync(GameVM gameDto)
        {
            try
            {
                var game = _mapper.Map<Game>(gameDto);
                return await _gameRepo.UpdateAsync(game);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int gameId)
        {
            try
            {
                return await _gameRepo.DeleteAsync(gameId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<(bool, List<GameVM>)> GetByCompanyAsync(int companyId)
        {
            try
            {
                var (success, games) = await _gameRepo.GetByCompanyAsync(companyId);
                return (success, _mapper.Map<List<GameVM>>(games));
            }
            catch (Exception ex)
            {
                return (false, new List<GameVM>());
            }
        }
    }
}
