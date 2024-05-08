namespace _4Dorms.Repositories.Interfaces
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(int dormitoryId, int studentId, int rating, string comment);
    }
}
