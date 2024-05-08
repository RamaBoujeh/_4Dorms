using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;

namespace _4Dorms.Repositories.implementation
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _genericRepository;
    }
}
