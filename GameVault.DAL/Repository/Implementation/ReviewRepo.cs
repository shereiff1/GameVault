using GameVault.DAL.Database;
using GameVault.DAL.Entities;
using GameVault.DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace GameVault.DAL.Repository.Implementation
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool, string?)> CreateAsync(Review review)
        {
            try
            {
                 _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return (true, "Review created successfully.");
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
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return (false, "Review not found.");
                }

                review.DELETE();
                await _context.SaveChangesAsync();

                return (true, "Review deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting review: {ex.Message}");
            }
        }

        public async Task<List<Review>> GetAllAsync()
        {
            try
            {
                return await _context.Reviews
                                     .Where(r => !r.IsDeleted)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving reviews: {ex.Message}");
            }
        }

        public async Task<(bool, string?)> UpdateAsync(Review review)
        {
            try
            {
                var rev = await _context.Reviews.FindAsync(review.Review_Id);
                if (rev == null)
                {
                    return (false, "Review not found.");
                }

                rev.Update(review.Review_Id, review.Player_Id, review.Comment, review.Rating);
                await _context.SaveChangesAsync();

                return (true, "Review updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating review: {ex.Message}");
            }
        }
        public async Task<(bool, Review?)> GetByIdAsync(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null || review.IsDeleted)
                {
                    return (false, null);
                }
                return (true, review);
            }
            catch (Exception ex)
            {
                return (false,null );
            }
        }
    }
}
