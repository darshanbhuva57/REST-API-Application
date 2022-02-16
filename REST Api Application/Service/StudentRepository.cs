using Microsoft.EntityFrameworkCore;
using REST_API_Application.Models;

namespace REST_Api_Application.Service
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;
        private readonly ILogger _logger;

        public StudentRepository(StudentContext context, ILoggerFactory loggerFactory)
        {
            this._context = context;
            _logger = loggerFactory
                      .CreateLogger<StudentRepository>();
        }
        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                _logger.LogError($"Student Not Found!!,Enter Valid Student Id");
                return null;
            }
            return student;
        }
        public async Task CreateStudent(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public void UpdateStudent(Student student, Student UpdateStudent)
        {
            student.FirstName = UpdateStudent.FirstName;
            student.LastName = UpdateStudent.LastName;
            student.Class = UpdateStudent.Class;
            student.Gender = UpdateStudent.Gender;
            student.BirthDate = UpdateStudent.BirthDate;
            student.MobileNumber = UpdateStudent.MobileNumber;
        }

        public void DeleteStudent(Student student)
        {
            _context.Students.Remove(student);
        }

        public async Task<List<Student>> GetStudentByName(string name)
        {
            var students = await _context.Students.Where(x => x.FirstName == name).ToListAsync();
            if (students.Count==0)
            {
                _logger.LogError($"Students Not Found!!,Enter Valid Student name");
                return null;
            }
            return students;
        }

        public async Task Save(string type)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error while {type} Student!!, {ex.Message}");
            }
        }


    }
}
