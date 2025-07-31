using GameVault_DAL.Entities;
using GameVault_DAL.Repo.Abstraction;

namespace GameVault_DAL.Repo.Implementation
{
    public class CategoryRepo : ICategoryRepo
    {
        // Connection string and database context would be injected here
        private readonly GameVaultDbContext db;
        public CategoryRepo()
        {
            // Initialize the database context here if needed
            this.db = new DbContext();
        }
        //dependency injection
        public CategoryRepo(DbContext db)
        {
            this.db = db;
        }
        public (bool, string?) Create(Category category)
        {
             try
             {
                 db.Categories.Add(category);
                 db.SaveChanges();
                 return (true, "Category created successfully.");
             }
             catch (Exception ex)
             {
                 return (false, $"Error creating category: {ex.Message}");
             }
             
        }

        public (bool, string?) Delete(int id)
        {            
             try
             {
                 var category = db.Categories.Find(id);
                 if (category == null)
                 {
                     return (false, "Category not found.");
                 }
                    category.DELETE(); 
                 db.SaveChanges();
                 return (true, "Category deleted successfully.");
             }
             catch (Exception ex)
             {
                 return (false, $"Error deleting category: {ex.Message}");
             }
             
        }

        public List<Category> GetAll()
        {
            
             try
             {
                 return db.Categories.Where(c => !c.IsDeleted).ToList();
             }
             catch (Exception ex)
             {
                 return new List<Category>();
             }
             
        }

        public (bool, string?) Update(Category category)
        {
            
             try
             {
                 var categ = db.Categories.Find(category.Category_Id);
                 if (categ == null)
                 {
                     return (false, "Category not found.");
                 }
                 categ.Update(category.Category_Id, category.Category_Name, category.Description);
                 db.SaveChanges();
                 return (true, "Category updated successfully.");
             }
             catch (Exception ex)
             {
                 return (false, $"Error updating category: {ex.Message}");
             }
             
        }
    }
}
