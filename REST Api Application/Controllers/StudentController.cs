using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using REST_Api_Application.Service;
using REST_API_Application.Models;

namespace REST_API_Application.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        private readonly ILogger _logger;

        public StudentController(ILoggerFactory loggerFactory, IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
            _logger = loggerFactory
                      .CreateLogger<StudentController>();
        }

        [HttpGet]
        public async Task<IActionResult> Student( )
        {
            var student = await studentRepository.GetAllStudents();
            if (student.Count == 0)
            {
                _logger.LogInformation($"Empty Databse!!");
                return Ok("Add some students");
            }
            return Ok(student);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Student(int id)
        {
            var student = await studentRepository.GetStudentById(id);
            if (student == null) return NotFound();

            return Ok(student);
        }

        [HttpGet("search")]
        public async Task<IActionResult> StudentsByName(string FirstName)
        {
            var student = await studentRepository.GetStudentByName(FirstName);
            if (student == null) return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Student(Student student)
        {
            if (student == null)
            {
                _logger.LogError($"Enter valid Student datails");
                return BadRequest();
            }

            await studentRepository.CreateStudent(student);

            await studentRepository.Save("Creating");

            return CreatedAtAction(
                nameof(Student),
                new { id = student.StudentId },
                student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Student(int id, Student UpdateStudent)
        {
            if (id != UpdateStudent.StudentId)
            {
                _logger.LogError($"Id does not match");
                return BadRequest();
            }

            var student = await studentRepository.GetStudentById(id);
            if (student == null) return NotFound();

            studentRepository.UpdateStudent(student, UpdateStudent);

            await studentRepository.Save("Updating");

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateStudent(int id,
             JsonPatchDocument<Student> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Null values");

            var student = await studentRepository.GetStudentById(id);
            if (student == null) return NotFound();

            patchDoc.ApplyTo(student, ModelState);

            TryValidateModel(student);

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Validation Error");
                return BadRequest(ModelState);
            }

            await studentRepository.Save("Updating");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await studentRepository.GetStudentById(id);
            if (student == null) return NotFound();

            studentRepository.DeleteStudent(student);

            await studentRepository.Save("Deleting");

            return NoContent();
        }
    }
}
