using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface ICategoryRepo
    {
        Task<(bool, string?)> CreateAsync(Category category);
        Task<(bool, List<Category>?)> GetAllAsync();
        Task<(bool, string?)> DeleteAsync(int id);
        Task<(bool, string?)> UpdateAsync(Category category);
        Task<(bool, Category?)> GetByIdAsync(int id);
    }
}
