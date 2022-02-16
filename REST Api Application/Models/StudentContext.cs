using Microsoft.EntityFrameworkCore;

namespace REST_API_Application.Models
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options)
           : base(options)
        {
            
        }

    }
}
