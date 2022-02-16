using REST_API_Application.Models;

namespace REST_Api_Application.Service
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudents();
        Task<Student> GetStudentById(int id);
        Task<List<Student>> GetStudentByName(string name);
        Task CreateStudent(Student student);
        void UpdateStudent(Student Student, Student UpdateStudent);
        void DeleteStudent(Student student);
        Task Save(string type);
    }
}
