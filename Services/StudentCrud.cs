using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace dotnetApi
{
    public static class StudentCrud
    {
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static IResult GetAllStudents(MySqlDbContext db)
        {
            var studentdata = db.Students.Select(s => new
            {
                s.StudentID,
                s.FirstName,
                s.LastName,
                s.EnrollmentDate,
                s.GraduationDate
            }).ToList();

            return Results.Ok(studentdata);
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public async static Task<IResult> CreateStudent(Student student_details, MySqlDbContext db)
        {
            try
            {
                // Check for null student details
                if (student_details == null)
                {
                    return Results.BadRequest("Student details cannot be null.");
                }

                // Validate the student object (optional, if you want to manually validate)
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(student_details, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(student_details, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return Results.BadRequest(new { Errors = errors });
                }

                // Add the new student to the database
                db.Students.Add(student_details);
                await db.SaveChangesAsync();
                return Results.Ok(student_details);
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
                Console.WriteLine($"Error creating student: {ex.Message}");
                return Results.Problem($"Failed to create student: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static async Task<IResult> DeleteStudent(int studentID, MySqlDbContext db)
        {
            try
            {
                // Log the student ID for debugging purposes
                Console.WriteLine(studentID);

                // Find the existing student by ID, including related StudentCourses
                var existingStudent = await db.Students.Include(s => s.StudentCourses).FirstOrDefaultAsync(s => s.StudentID == studentID);
                if (existingStudent == null)
                {
                    return Results.NotFound("Student not found");
                }

                // Remove associated StudentCourses if they exist
                if (existingStudent.StudentCourses != null && existingStudent.StudentCourses.Any())
                {
                    db.StudentCourses.RemoveRange(existingStudent.StudentCourses);
                }

                // Remove the student
                db.Students.Remove(existingStudent);
                await db.SaveChangesAsync();

                return Results.Ok("Student deleted successfully.");
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
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return Results.Problem($"Failed to delete student: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static async Task<IResult> UpdateStudent(Student updatedStudent, MySqlDbContext db)
        {
            try
            {
                // Check for null student details
                if (updatedStudent == null)
                {
                    return Results.BadRequest("Student details cannot be null.");
                }

                // Find the existing student by ID
                var existingStudent = await db.Students.FirstOrDefaultAsync(s => s.StudentID == updatedStudent.StudentID);
                if (existingStudent == null)
                {
                    return Results.NotFound("Student not found");
                }

                // Update the student properties
                existingStudent.FirstName = updatedStudent.FirstName;
                existingStudent.LastName = updatedStudent.LastName;
                existingStudent.EnrollmentDate = updatedStudent.EnrollmentDate;
                existingStudent.GraduationDate = updatedStudent.GraduationDate;

                // If you need to update StudentCourses, handle it as needed here.
                // For example, if you want to replace existing courses:
                // existingStudent.StudentCourses.Clear();
                // existingStudent.StudentCourses.AddRange(updatedStudent.StudentCourses);

                // Save changes to the database
                await db.SaveChangesAsync();

                return Results.Ok("Student updated successfully.");
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
                Console.WriteLine($"Error updating student: {ex.Message}");
                return Results.Problem($"Failed to update student: {ex.Message}", statusCode: 500);
            }
        }
    }
}