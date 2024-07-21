using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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
                // Log received instructor object
                Console.WriteLine($"Received Instructor: {JsonSerializer.Serialize(instructor)}");

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
                    Console.WriteLine($"Validation errors: {JsonSerializer.Serialize(errors)}");
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
                // Log received instructor object
                Console.WriteLine($"Received Instructor: {JsonSerializer.Serialize(instructor)}");

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
                // Log instructor ID to be deleted
                Console.WriteLine($"Deleting Instructor ID: {instructorID}");

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
            try
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

                Console.WriteLine($"Retrieved Instructors: {JsonSerializer.Serialize(instructorData)}");

                return Results.Ok(instructorData);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error retrieving instructors: {ex.Message}");
                return Results.Problem($"Failed to retrieve instructors: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> GetInstructorById(int instructorID, MySqlDbContext db)
        {
            try
            {
                // Log instructor ID to be retrieved
                Console.WriteLine($"Retrieving Instructor ID: {instructorID}");

                var instructor = await db.Instructors
                    .Include(i => i.Department)
                    .FirstOrDefaultAsync(i => i.InstructorID == instructorID);

                if (instructor == null)
                {
                    return Results.NotFound("Instructor not found");
                }

                var result = new
                {
                    instructor.InstructorID,
                    instructor.FirstName,
                    instructor.LastName,
                    instructor.Status,
                    instructor.HireDate,
                    instructor.AnnualSalary,
                    instructor.DepartmentID
                };

                Console.WriteLine($"Retrieved Instructor: {JsonSerializer.Serialize(result)}");

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error retrieving instructor: {ex.Message}");
                return Results.Problem($"Failed to retrieve instructor: {ex.Message}", statusCode: 500);
            }
        }
    }
}
