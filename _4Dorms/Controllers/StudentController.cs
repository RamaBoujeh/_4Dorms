using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _4Dorms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IUserService _userService; // Ensure this is properly injected
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, IUserService userService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _userService = userService; // Assign the injected service
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching students");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(int studentId)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(studentId);
                if (student == null)
                {
                    return NotFound();
                }
                return Ok(student);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching student");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var result = await _userService.DeleteUserProfileAsync(studentId, UserType.Student);

            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
