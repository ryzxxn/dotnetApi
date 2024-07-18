using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using dotnetApi;

namespace dotnetApi.Functions
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

    public async static Task<IResult> CreateStudent(Student student, MySqlDbContext db)
        {
            try
            {
                Console.WriteLine(student.FirstName);
                db.Students.Add(student);
                await db.SaveChangesAsync();
                return Results.Ok(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating student: {ex.Message}");
                return Results.BadRequest("Failed to create student.");
            }

        }

    public static async Task<IResult> DeleteStudent(Student student, MySqlDbContext db)
            {
                var existingStudent = await db.Students.Include(s => s.StudentCourses).FirstOrDefaultAsync(s => s.StudentID == student.StudentID);
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

    public static async Task<IResult> UpdateStudent(Student updatedStudent, MySqlDbContext db)
        {
            var existingStudent = await db.Students.Include(s => s.StudentCourses).FirstOrDefaultAsync(s => s.StudentID == updatedStudent.StudentID);
            if (existingStudent == null)
                {
                    return Results.NotFound("Student not found");
                }

            if (updatedStudent.FirstName == null || updatedStudent.LastName == null || updatedStudent.GraduationDate == null || updatedStudent.EnrollmentDate == null)
                {
                    return Results.BadRequest("Missing Values");
                }

            existingStudent.FirstName = updatedStudent.FirstName;
            existingStudent.LastName = updatedStudent.LastName;
            existingStudent.EnrollmentDate = updatedStudent.EnrollmentDate;
            existingStudent.GraduationDate = updatedStudent.GraduationDate;

            if (updatedStudent.StudentCourses != null)
                {
                    // Remove existing student courses
                    db.StudentCourses.RemoveRange(existingStudent.StudentCourses);

                    // Add updated student courses
                    foreach (var studentCourse in updatedStudent.StudentCourses)
                    {
                        db.StudentCourses.Add(studentCourse);
                    }
                }

                await db.SaveChangesAsync();
                return Results.Ok();
        }
    }
}
