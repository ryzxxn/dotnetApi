using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnetApi
{
    public static class CourseCrud
    {
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> CreateCourse(Course course, MySqlDbContext db)
        {
            try
            {
                // Check for null course
                if (course == null)
                {
                    return Results.BadRequest("Course details cannot be null.");
                }

                // Validate the course object (optional, if you want to manually validate)
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(course, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(course, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return Results.BadRequest(new { Errors = errors });
                }

                // Add the new course to the database
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return Results.Ok(course);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Results.Problem($"Database update error: {dbEx.Message}", statusCode: 500);
            }
            catch (ValidationException valEx)
            {
                // Handle validation exceptions
                Console.WriteLine($"Validation error: {valEx.Message}");
                return Results.BadRequest($"Validation error: {valEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error creating course: {ex.Message}");
                return Results.Problem($"Failed to create course: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> UpdateCourse(Course course, MySqlDbContext db)
        {
            try
            {
                var existingCourse = await db.Courses.FindAsync(course.CourseID);
                if (existingCourse == null)
                {
                    return Results.NotFound("Course not found");
                }

                existingCourse.CourseName = course.CourseName;
                existingCourse.CourseDescription = course.CourseDescription;
                existingCourse.CourseUnits = course.CourseUnits;
                existingCourse.DepartmentID = course.DepartmentID;
                existingCourse.InstructorID = course.InstructorID;

                await db.SaveChangesAsync();
                return Results.Ok(existingCourse);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Results.Problem($"Database update error: {dbEx.Message}", statusCode: 500);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error updating course: {ex.Message}");
                return Results.Problem($"Failed to update course: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> DeleteCourse(int courseID, MySqlDbContext db)
        {
            try
            {
                var existingCourse = await db.Courses
                    .Include(c => c.Department) // Include related department if needed
                    .Include(c => c.Instructor) // Include related instructor if needed
                    .FirstOrDefaultAsync(c => c.CourseID == courseID);

                if (existingCourse == null)
                {
                    return Results.NotFound("Course not found");
                }

                db.Courses.Remove(existingCourse);
                await db.SaveChangesAsync();

                return Results.Ok("Course deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Results.Problem($"Database update error: {dbEx.Message}", statusCode: 500);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error deleting course: {ex.Message}");
                return Results.Problem($"Failed to delete course: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IResult GetAllCourses(MySqlDbContext db)
        {
            var courseData = db.Courses.Select(c => new
            {
                c.CourseID,
                c.CourseName,
                c.CourseDescription,
                c.CourseUnits,
                c.DepartmentID,
                c.InstructorID
            }).ToList();

            return Results.Ok(courseData);
        }
    }
}
