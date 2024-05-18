using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;

namespace _4Dorms.Repositories.implementation
{
    public class ReviewService  : IReviewService
    {
        private readonly IGenericRepository<Review> _ReviewRepository;
        private readonly IGenericRepository<Booking> _BookingRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IGenericRepository<Review> ReviewRepository, ILogger<ReviewService> logger, IGenericRepository<Booking> BookingRepository)
        {
            _ReviewRepository = ReviewRepository;
            _logger = logger;
            _BookingRepository = BookingRepository;

        }

        public async Task<bool> AddReviewAsync(int dormitoryId, int studentId, int rating, string comment)
        {
            // Check if the student has booked the specified dormitory
            var booking = await _BookingRepository.FindByConditionAsync(b => b.DormitoryId == dormitoryId);
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
