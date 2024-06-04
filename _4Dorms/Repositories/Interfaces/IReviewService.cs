using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(int dormitoryId, int studentId, int rating, string comment);
        Task<List<ReviewDTO>> GetReviewsByDormitoryAsync(int dormitoryId);
    }
}
