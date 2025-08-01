using GameVault_BLL.ModelVM.Review;

namespace GameVault_BLL.Services.Abstraction
{
    public interface IReviewServices
    {
        Task<(bool, string?)> CreateAsync(CreateReview review);
        Task<(bool, string?)> UpdateAsync(UpdateReview review);
        Task<(bool, string?)> DeleteAsync(int id);
        Task<List<ReviewDTO>> GetAllAsync();

    }
}
