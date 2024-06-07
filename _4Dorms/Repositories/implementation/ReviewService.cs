using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4Dorms.Repositories.implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IGenericRepository<Review> reviewRepository, ILogger<ReviewService> logger, IGenericRepository<Booking> bookingRepository)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> AddReviewAsync(int dormitoryId, int studentId, int rating, string comment)
        {
            var booking = await _bookingRepository.FindByConditionAsync(b => b.DormitoryId == dormitoryId && b.StudentId == studentId);
            if (booking == null)
            {
                _logger.LogWarning($"Student with ID {studentId} cannot leave a review for dormitory with ID {dormitoryId} as they haven't booked it.");
                return false;
            }

            try
            {
                var review = new Review
                {
                    DormitoryId = dormitoryId,
                    StudentId = studentId,
                    Rating = rating,
                    Comment = comment,
                    Date = DateTime.UtcNow
                };

                await _reviewRepository.Add(review);
                await _reviewRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add review: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ReviewDTO>> GetReviewsByDormitoryAsync(int dormitoryId)
        {
            var reviews = await _reviewRepository.GetListByConditionAsync(r => r.DormitoryId == dormitoryId);

            _logger.LogInformation($"Reviews fetched from the database: {reviews.Count}");
            foreach (var review in reviews)
            {
                _logger.LogInformation($"ReviewId: {review.ReviewId}, DormitoryId: {review.DormitoryId}, StudentId: {review.StudentId}, Comment: {review.Comment}");
            }

            return reviews.Select(r => new ReviewDTO
            {
                DormitoryId = r.DormitoryId ?? 0,
                StudentId = r.StudentId ?? 0,
                Rating = r.Rating,
                Comment = r.Comment,
                Date = r.Date,
                StudentName = r.Student != null ? r.Student.Name : "Unknown"
            }).ToList();
        }

    }
}
