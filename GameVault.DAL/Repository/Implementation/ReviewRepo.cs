using GameVault_DAL.Entities;
using GameVault_DAL.Repo.Abstraction;
﻿using Microsoft.EntityFrameworkCore;
using GameVault_DAL.Database;

namespace GameVault_DAL.Repo.Implementation
{
    public class ReviewRepo : IReviewRepo
    {
        //Cnnection string and database context would be injected here
        private readonly DbContext db;
        public ReviewRepo() 
        {
            // Initialize the database context here if needed
            this.db = new DbContext();
        }
        public ReviewRepo(DbContext db)
        {
           this.db = db;
        }

        public (bool, string?) Create(Review review)
        {            
             try
             {
                 db.Reviews.Add(review);
                 db.SaveChanges();
                 return (true, "Review created successfully.");
             }
             catch (Exception ex)
             {
                 return (false, $"Error creating review: {ex.Message}");
             }
        }

        public (bool, string?) Delete(int id)
        {
             try
             {
                 var review = db.Reviews.Find(id);
                 if (review == null)
                 {
                     return (false, "Review not found.");
                 }
                 review.DELETE(); 
                 db.SaveChanges();
                 return (true, "Review deleted successfully.");
             }
             catch (Exception ex)
             {
                 return (false, $"Error deleting review: {ex.Message}");
             }
        }

        public List<Review> GetAll()
        {
             try
             {
                 return db.Reviews.Where(r => !r.IsDeleted).ToList();
             }
             catch (Exception ex)
             {
                throw new Exception("Error retrieving reviews: " + ex.Message);
             }
        }

        public (bool, string?) Update(Review review)
        {
             try
             {
                 var rev = db.Reviews.Find(review.Review_Id);
                 if (rev == null)
                 {
                     return (false, "Review not found.");
                 }
                 rev.Update(review.Review_Id, review.Player_Id, review.Comment, review.Rating);
                 db.SaveChanges();
                 return (true,null);
             }
             catch (Exception ex)
             {
                 return (false, $"Error updating review: {ex.Message}");
             }
        }
    }
}
