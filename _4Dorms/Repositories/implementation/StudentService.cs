using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _4Dorms.Repositories.implementation
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _genericRepositoryStudent;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IGenericRepository<Student> genericRepositoryStudent, ILogger<StudentService> logger)
        {
            _genericRepositoryStudent = genericRepositoryStudent;
            _logger = logger;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _genericRepositoryStudent.GetAllAsync();
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int studentId)
        {
            var student = await _genericRepositoryStudent.GetByIdAsync(studentId);
            if (student == null) return null;

            return new StudentDTO
            {
                Name = student.Name,
                Email = student.Email,
                Password = student.Password,
                PhoneNumber = student.PhoneNumber,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                Disabilities = student.Disabilities
            };
        }


    }
}
