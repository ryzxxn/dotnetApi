using Microsoft.EntityFrameworkCore;

namespace dotnetApi
{
    public class MySqlDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; } // Use plural for DbSet property names
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Instructor> Departments { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure MySQL connection
            optionsBuilder.UseMySql("Server=localhost;Database=college;User=root;", 
                new MySqlServerVersion(new Version(8, 0, 27)));
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configure relationships
                modelBuilder.Entity<StudentCourse>().ToTable("studentscourses");
                modelBuilder.Entity<Student>().ToTable("students");
                modelBuilder.Entity<Course>().ToTable("courses");
                modelBuilder.Entity<Department>().ToTable("departments");
                modelBuilder.Entity<Instructor>().ToTable("instructors");
                
                modelBuilder.Entity<StudentCourse>()
                    .HasKey(sc => new { sc.StudentID, sc.CourseID });

                modelBuilder.Entity<StudentCourse>()
                    .HasOne(sc => sc.Student)
                    .WithMany(s => s.StudentCourses)
                    .HasForeignKey(sc => sc.StudentID);

                modelBuilder.Entity<StudentCourse>()
                    .HasOne(sc => sc.Course)
                    .WithMany(c => c.StudentCourses)
                    .HasForeignKey(sc => sc.CourseID);

                modelBuilder.Entity<Course>()
                    .HasOne(c => c.Department)
                    .WithMany(d => d.Courses)
                    .HasForeignKey(c => c.DepartmentID);

                modelBuilder.Entity<Course>()
                    .HasOne(c => c.Instructor)
                    .WithMany(i => i.Courses)
                    .HasForeignKey(c => c.InstructorID);
            }
    }
}
