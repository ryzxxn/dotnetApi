using Microsoft.EntityFrameworkCore;

namespace dotnetApi // Namespace declaration should be outside the class definition
{
    public class MySqlDbContext : DbContext
    {
        public DbSet<Student> Student { get; set; } // Use plural for DbSet property names

        public DbSet<Course> Course { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure MySQL connection
            optionsBuilder.UseMySql("Server=localhost;Database=college;User=root;", 
                new MySqlServerVersion(new Version(8, 0, 27))); // Updated to MySqlServerVersion
        }
    }
}
