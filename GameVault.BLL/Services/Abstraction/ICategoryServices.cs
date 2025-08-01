using GameVault_BLL.ModelVM.Category;
using GameVault_DAL.Entities;

namespace GameVault_BLL.Services.Abstraction
{
    public interface ICategoryServices
    {
        Task<(bool, string?)> CreateAsync(CreateCategory category);
        Task<(bool, string?)> DeleteAsync(int id);
        Task<List<CategoryDTO>> GetAllAsync();
        Task<(bool, string?)> UpdateAsync(UpdateCategory category);
    }
}
