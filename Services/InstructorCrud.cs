using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnetApi
{
    public static class InstructorCrud
    {
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> CreateInstructor(Instructor instructor, MySqlDbContext db)
        {
            try
            {
                // Check for null instructor
                if (instructor == null)
                {
                    return Results.BadRequest("Instructor details cannot be null.");
                }

                // Validate the instructor object (optional, if you want to manually validate)
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(instructor, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(instructor, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return Results.BadRequest(new { Errors = errors });
                }

                // Add the new instructor to the database
                db.Instructors.Add(instructor);
                await db.SaveChangesAsync();
                return Results.Ok(instructor);
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
                Console.WriteLine($"Error creating instructor: {ex.Message}");
                return Results.Problem($"Failed to create instructor: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> UpdateInstructor(Instructor instructor, MySqlDbContext db)
        {
            try
            {
                var existingInstructor = await db.Instructors.FindAsync(instructor.InstructorID);
                if (existingInstructor == null)
                {
                    return Results.NotFound("Instructor not found");
                }

                existingInstructor.FirstName = instructor.FirstName;
                existingInstructor.LastName = instructor.LastName;
                existingInstructor.Status = instructor.Status;
                existingInstructor.HireDate = instructor.HireDate;
                existingInstructor.AnnualSalary = instructor.AnnualSalary;
                existingInstructor.DepartmentID = instructor.DepartmentID;

                await db.SaveChangesAsync();
                return Results.Ok(existingInstructor);
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
                Console.WriteLine($"Error updating instructor: {ex.Message}");
                return Results.Problem($"Failed to update instructor: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> DeleteInstructor(int instructorID, MySqlDbContext db)
        {
            try
            {
                var existingInstructor = await db.Instructors
                    .Include(i => i.Department) // Include related department if needed
                    .FirstOrDefaultAsync(i => i.InstructorID == instructorID);

                if (existingInstructor == null)
                {
                    return Results.NotFound("Instructor not found");
                }

                db.Instructors.Remove(existingInstructor);
                await db.SaveChangesAsync();

                return Results.Ok("Instructor deleted successfully.");
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
                Console.WriteLine($"Error deleting instructor: {ex.Message}");
                return Results.Problem($"Failed to delete instructor: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IResult GetAllInstructors(MySqlDbContext db)
        {
            var instructorData = db.Instructors.Select(i => new
            {
                i.InstructorID,
                i.FirstName,
                i.LastName,
                i.Status,
                i.HireDate,
                i.AnnualSalary,
                i.DepartmentID
            }).ToList();

            return Results.Ok(instructorData);
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> GetInstructorById(int instructorID, MySqlDbContext db)
        {
            var instructor = await db.Instructors
                .Include(i => i.Department)
                .FirstOrDefaultAsync(i => i.InstructorID == instructorID);

            if (instructor == null)
            {
                return Results.NotFound("Instructor not found");
            }

            return Results.Ok(new
            {
                instructor.InstructorID,
                instructor.FirstName,
                instructor.LastName,
                instructor.Status,
                instructor.HireDate,
                instructor.AnnualSalary,
                instructor.DepartmentID
            });
        }
    }
}
