using GameVault_BLL.ModelVM.Category;
using GameVault_BLL.Services.Abstraction;
using GameVault_DAL.Entities;
using GameVault_DAL.Repository.Abstraction;
using AutoMapper;


namespace GameVault_BLL.Services.Implementation
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryServices(ICategoryRepo categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task<(bool, string?)> CreateAsync(CreateCategory category)
        {
              try
              {
                  var categ = _mapper.Map<Category>(category);
                  var result = await _categoryRepo.CreateAsync(categ);
                  return result;
              }
              catch (Exception ex)
              {
                  return (false, $"Error creating category: {ex.Message}");
              }
        }

        public async Task<(bool, string?)> DeleteAsync(int id)
        {
              try
              {
                  var result = await _categoryRepo.DeleteAsync(id);
                  return result;
              }
              catch (Exception ex)
              {
                  return (false, $"Error deleting category: {ex.Message}");
              }
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
              try
              {
                  var categories = await _categoryRepo.GetAllAsync();
                  var mappedCategories = _mapper.Map<List<CategoryDTO>>(categories);
                  return (mappedCategories, null);
              }
              catch (Exception ex)
              {
                  return (null, ex.Message);
             }
        }

        public async Task<(bool, string?)> UpdateAsync(UpdateCategory category)
        {
              try
              {
                  var categ = _mapper.Map<Category>(category);
                  var result = await _categoryRepo.UpdateAsync(categ);
                  return result;
              }
              catch (Exception ex)
              {
                  return (false, $"Error updating category: {ex.Message}");
              }
        }
      }
}
