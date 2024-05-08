using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;

namespace _4Dorms.Repositories.implementation
{
    public class ReviewService  : IReviewService
    {
        private readonly IGenericRepository<Review> _ReviewRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IGenericRepository<Review> ReviewRepository, ILogger<ReviewService> logger)
        {
            _ReviewRepository = ReviewRepository;
            _logger = logger;

        }

        public async Task<bool> AddReviewAsync(int dormitoryId, int studentId, int rating, string comment)
        {
            var dormitory = await _ReviewRepository.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                return false;
            }

            var student = await _ReviewRepository.GetByIdAsync(studentId);
            if (student == null)
            {
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

                _ReviewRepository.Add(review);

                await _ReviewRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add review: {ex.Message}");
                return false;
            }
            
        }
    }
}
