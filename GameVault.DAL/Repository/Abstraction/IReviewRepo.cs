using GameVault_DAL.Entities;

namespace GameVault_DAL.Repo.Abstraction
{
    public interface IReviewRepo
    {
        (bool, string?) Create(Review Dept);
        List<Review> GetAll();
        (bool, string?) Delete(int id);
        (bool, string?) Update(Review Dept);
    }
}
