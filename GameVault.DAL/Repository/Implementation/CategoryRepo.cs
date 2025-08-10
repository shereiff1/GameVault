using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace GameVault.DAL.Repository.Implementation
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
                _context.Categories.Add(category);
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
                var category = await _context.Categories.FindAsync(id);
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


        public async Task<(bool, List<Category>?)> GetAllAsync()
        {
            try
            {
                var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
                return (true, categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving categories: {ex.Message}");
                return (false, null);
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
                existingCategory.Update(category.Category_Id, category.Category_Name, category.Description,category.CreatedBy);
                await _context.SaveChangesAsync();
                return (true, "Category updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating category: {ex.Message}");
            }
        }
        public async Task<(bool, Category?)> GetByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null || category.IsDeleted)
                {
                    return (false, null);
                }
                return (true, category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving category by ID: {ex.Message}");
                return (false, null);
            }
        }
    }
}
