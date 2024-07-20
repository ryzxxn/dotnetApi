using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnetApi
{
    public static class DepartmentCrud
    {
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IResult GetAllDepartments(MySqlDbContext db)
        {
            var departmentData = db.Departments.Select(d => new
            {
                d.DepartmentID,
                d.DepartmentName
            }).ToList();

            return Results.Ok(departmentData);
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async static Task<IResult> CreateDepartment(Department department, MySqlDbContext db)
        {
            try
            {
                // Check for null department
                if (department == null)
                {
                    return Results.BadRequest("Department details cannot be null.");
                }

                // Validate the department object (optional, if you want to manually validate)
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(department, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(department, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return Results.BadRequest(new { Errors = errors });
                }

                // Check if the department name already exists
                var existingDepartment = await db.Departments.FirstOrDefaultAsync(d => d.DepartmentName == department.DepartmentName);
                if (existingDepartment != null)
                {
                    return Results.BadRequest("A department with this name already exists.");
                }

                // Add the new department to the database
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return Results.Ok(department);
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
                Console.WriteLine($"Error creating department: {ex.Message}");
                return Results.Problem($"Failed to create department: {ex.Message}", statusCode: 500);
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async static Task<IResult> UpdateDepartment(Department department, MySqlDbContext db)
        {
            var existingDepartment = await db.Departments.FindAsync(department.DepartmentID);
            if (existingDepartment == null)
            {
                return Results.NotFound("Department not found");
            }

            existingDepartment.DepartmentName = department.DepartmentName;
            await db.SaveChangesAsync();

            return Results.Ok(existingDepartment);
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<IResult> DeleteDepartment(int departmentID, MySqlDbContext db)
        {
            try
            {
                // Find the existing department by ID, including related entities if necessary
                var existingDepartment = await db.Departments
                    .Include(d => d.Courses)    // Include related courses if needed
                    .Include(d => d.Instructors) // Include related instructors if needed
                    .FirstOrDefaultAsync(d => d.DepartmentID == departmentID);

                if (existingDepartment == null)
                {
                    return Results.NotFound("Department not found");
                }

                // Remove associated courses and instructors if needed
                if (existingDepartment.Courses != null && existingDepartment.Courses.Any())
                {
                    db.Courses.RemoveRange(existingDepartment.Courses);
                }
                if (existingDepartment.Instructors != null && existingDepartment.Instructors.Any())
                {
                    db.Instructors.RemoveRange(existingDepartment.Instructors);
                }

                // Remove the department
                db.Departments.Remove(existingDepartment);
                await db.SaveChangesAsync();

                return Results.Ok("Department deleted successfully.");
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
                Console.WriteLine($"Error deleting department: {ex.Message}");
                return Results.Problem($"Failed to delete department: {ex.Message}", statusCode: 500);
            }
        }
    }
}
