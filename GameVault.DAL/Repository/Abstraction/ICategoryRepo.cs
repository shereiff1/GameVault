using GameVault_DAL.Entities;

namespace GameVault_DAL.Repo.Abstraction
{
    public interface ICategoryRepo
    {
        (bool, string?) Create(Category Dept);
        List<Category> GetAll();
        (bool, string?) Delete(int id);
        (bool, string?) Update(Category Dept);
    }
}
