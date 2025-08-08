using GameVault.BLL.ModelVM.Review;

namespace GameVault.BLL.Services.Abstraction
{
    public interface IReviewServices
    {
        Task<(bool, string?)> CreateAsync(CreateReview review);
        Task<(bool, string?)> UpdateAsync(UpdateReview review);
        Task<(bool, string?)> DeleteAsync(int id);
        Task<(bool, List<ReviewDTO>?)> GetAllAsync();
        Task<(bool, ReviewDTO?)> GetByIdAsync(int id);

    }
}
