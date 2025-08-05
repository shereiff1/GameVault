using GameVault.BLL.ModelVM.Review;
using GameVault.BLL.Services.Abstraction;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using AutoMapper;
using GameVault.DAL.Repo.Abstraction;

namespace GameVault.BLL.Services.Implementation
{
    public class ReviewServices : IReviewServices
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IMapper _mapper;
        public ReviewServices(IReviewRepo reviewRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        public async Task<(bool, string?)> CreateAsync(CreateReview review)
        {
            try
            {
                var rev = _mapper.Map<Review>(review);
                var result = await _reviewRepo.CreateAsync(rev);
                return result;
            }
            catch (Exception ex)
            {
                return (false, $"Error creating review: {ex.Message}");
            }
        }

        public async Task<(bool, string?)> DeleteAsync(int id)
        {
            try
            {
                var result = await _reviewRepo.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting review: {ex.Message}");
            }
        }

        public async Task<(bool, List<ReviewDTO>?)> GetAllAsync()
        {
            try
            {
                var reviews = await _reviewRepo.GetAllAsync();
                var mappedReviews = _mapper.Map<List<ReviewDTO>>(reviews);
                return (true, mappedReviews);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (false, null);
            }
        }

        public async Task<(bool, string?)> UpdateAsync(UpdateReview review)
        {
            try
            {
                var rev = _mapper.Map<Review>(review);
                var result = await _reviewRepo.UpdateAsync(rev);
                return result;
            }
            catch (Exception ex)
            {
                return (false, $"Error updating review: {ex.Message}");
            }
        }
    }
}
