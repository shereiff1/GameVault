using GameVault.BLL.ModelVM.Category;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using AutoMapper;
using GameVault.DAL.Repo.Abstraction;


namespace GameVault.BLL.Services.Implementation
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

        public async Task<(bool,List<CategoryDTO>?)> GetAllAsync()
        {
              try
              {
                  var categories = await _categoryRepo.GetAllAsync();
                  var mappedCategories = _mapper.Map<List<CategoryDTO>>(categories);
                  return (true, mappedCategories);
              }
              catch (Exception ex)
              {
                Console.WriteLine(ex.Message);
                  return (false,null);
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
