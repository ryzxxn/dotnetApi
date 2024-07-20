using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnetApi
{
    public static class DepartmentCrud
    {
        public static IResult GetAllDepartments(MySqlDbContext db)
        {
            var departmentData = db.Departments.Select(d => new
            {
                d.DepartmentID,
                d.DepartmentName
            }).ToList();

            return Results.Ok(departmentData);
        }

        public async static Task<IResult> CreateDepartment(Department department, MySqlDbContext db)
        {
            try
            {
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return Results.Ok(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating department: {ex.Message}");
                return Results.BadRequest($"Failed to create department.: {ex.Message}");
            }
        }

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

        public async static Task<IResult> DeleteDepartment(int departmentId, MySqlDbContext db)
        {
            var existingDepartment = await db.Departments.Include(d => d.Courses).FirstOrDefaultAsync(d => d.DepartmentID == departmentId);
            if (existingDepartment == null)
            {
                return Results.NotFound("Department not found");
            }

            if (existingDepartment.Courses != null)
            {
                db.Courses.RemoveRange(existingDepartment.Courses);
            }

            db.Departments.Remove(existingDepartment);
            await db.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
