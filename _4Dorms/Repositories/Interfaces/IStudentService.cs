using _4Dorms.Models;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
    }
}
    