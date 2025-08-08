using GameVault.BLL.ModelVM.Category;
using GameVault.DAL.Entities;

namespace GameVault.BLL.Services.Abstraction
{
    public interface ICategoryServices
    {
        Task<(bool, string?)> CreateAsync(CreateCategory category);
        Task<(bool, string?)> DeleteAsync(int id);
        Task<(bool, List<CategoryDTO>?)> GetAllAsync();
        Task<(bool, string?)> UpdateAsync(UpdateCategory category);
        Task<(bool, CategoryDTO?)> GetByIdAsync(int id);
    }
}
