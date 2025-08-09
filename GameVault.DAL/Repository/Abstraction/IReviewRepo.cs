using GameVault.DAL.Entities;

namespace GameVault.DAL.Repository.Abstraction
{
    public interface IReviewRepo
    {
        Task<(bool, string?)> CreateAsync(Review review);
        Task<(bool, List<Review>?)> GetAllAsync();
        Task<(bool, string?)> DeleteAsync(int id);
        Task<(bool, string?)> UpdateAsync(Review review);
        Task<(bool, Review?)> GetByIdAsync(int id);

    }
}
