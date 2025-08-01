using GameVault_DAL.Entities;

namespace GameVault_DAL.Repo.Abstraction
{
    public interface ICategoryRepo
    {
        Task<(bool, string?)> CreateAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task<(bool, string?)> DeleteAsync(int id);
        Task<(bool, string?)> UpdateAsync(Category category);
    }
}
