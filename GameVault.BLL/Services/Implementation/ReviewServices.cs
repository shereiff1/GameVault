using GameVault_BLL.ModelVM.Review;
using GameVault_BLL.Services.Abstraction;
using GameVault_DAL.Entities;
using GameVault_DAL.Repository.Abstraction;
using AutoMapper;

namespace GameVault_BLL.Services.Implementation
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

        public async Task<List<ReviewDTO>> GetAllAsync()
        {
              try
              {
                  var reviews = await _reviewRepo.GetAllAsync();
                  var mappedReviews = _mapper.Map<List<ReviewDTO>>(reviews);
                  return (mappedReviews, null);
              }
              catch (Exception ex)
              {
                  return (null, ex.Message);
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
