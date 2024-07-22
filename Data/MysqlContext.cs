using Microsoft.EntityFrameworkCore;

namespace dotnetApi
{
    public class MySqlDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserUserRole> UserUserRole { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure MySQL connection
            optionsBuilder.UseMySql("Server=localhost;Database=university;User=root;", 
                new MySqlServerVersion(new Version(8, 0, 27)));
        }

        public MySqlDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}