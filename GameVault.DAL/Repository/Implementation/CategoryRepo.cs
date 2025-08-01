using GameVault_DAL.Entities;
using GameVault_DAL.Repo.Abstraction;

namespace GameVault_DAL.Repo.Implementation
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool, string?)> CreateAsync(Category category)
        {
             try
             {
                 await _context.Categories.Add(category);
                 await _context.SaveChangesAsync();
                 return (true, "Category created successfully.");
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
                 var category =await _context.Categories.Find(id);
                 if (category == null)
                 {
                     return (false, "Category not found.");
                 }
                 category.DELETE();
                 await _context.SaveChangesAsync();
                 return (true, "Category deleted successfully.");
             }
             catch (Exception ex)
             {
                 return (false, $"Error deleting category: {ex.Message}");
             }
        }

        public async Task<List<Category>> GetAllAsync()
        {
             try
             {
                 return await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error retrieving categories: {ex.Message}");
                 return (false,null);
             }
        }

        public async Task<(bool, string?)> UpdateAsync(Category category)
        {
             try
             {
                 var existingCategory = await _context.Categories.FindAsync(category.Category_Id);
                 if (existingCategory == null)
                 {
                     return (false, "Category not found.");
                 }
                 existingCategory.Update(category.Category_Id, category.Category_Name, category.Description);
                 await _context.SaveChangesAsync();
                 return (true, "Category updated successfully.");
             }
             catch (Exception ex)
             {
                 return (false, $"Error updating category: {ex.Message}");
             }
        }
    }
}
