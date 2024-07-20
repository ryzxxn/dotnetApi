using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnetApi
{
    public static class StudentCrud
    {
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

    public async static Task<IResult> CreateStudent(Student student_details, MySqlDbContext db)
        {
            try
            {
                // Console.WriteLine(student_details.FirstName);
                var newStudent = new {
                    student_details.FirstName,
                    student_details.LastName,
                    student_details.EnrollmentDate,
                    student_details.GraduationDate,
                };
                db.Students.Add(student_details);
                await db.SaveChangesAsync();
                return Results.Ok(student_details);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating student: {ex.Message}");
                return Results.BadRequest($"Failed to create student.: {ex.Message}");
            }

        }
    
    public static async Task<IResult> DeleteStudent(Student student_details, MySqlDbContext db)
        {
            Console.WriteLine(student_details.StudentID);
            var existingStudent = await db.Students.Include(s => s.StudentCourses).FirstOrDefaultAsync(s => s.StudentID == student_details.StudentID);
            if (existingStudent == null)
            {
                return Results.NotFound("Student not found");
            }

            if (existingStudent.StudentCourses != null)
            {
                db.StudentCourses.RemoveRange(existingStudent.StudentCourses);
            }

            db.Students.Remove(existingStudent);
            await db.SaveChangesAsync();

            return Results.Ok();
        }
    }
}