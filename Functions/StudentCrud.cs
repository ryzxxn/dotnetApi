using Microsoft.AspNetCore.Http;
using dotnetApi;

namespace dotnetApi.Functions
{
    public static class StudentCrud
    {
        public static IResult GetAllStudents(MySqlDbContext db)
        {
            var studentdata = db.Students.ToList();
            return Results.Ok(studentdata);
        }

        public static async Task<IResult> CreateStudent(Student student, MySqlDbContext db)
        {
            try
            {
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
            var existingStudent = await db.Students.FindAsync(student.StudentID);
            if (existingStudent == null)
            {
                return Results.NotFound("Student not found");
            }

            db.Students.Remove(existingStudent);
            await db.SaveChangesAsync();

            return Results.Ok();
        }

        public static async Task<IResult> UpdateStudent(Student updatedStudent, MySqlDbContext db)
        {
            var existingStudent = await db.Students.FindAsync(updatedStudent.StudentID);
            if (existingStudent == null)
            {
                return Results.NotFound("Student not found");
            }

            if (updatedStudent.FirstName == null || updatedStudent.LastName == null ||
                updatedStudent.GraduationDate == null || updatedStudent.EnrollmentDate == null)
            {
                return Results.BadRequest("Missing Values");
            }

            existingStudent.FirstName = updatedStudent.FirstName;
            existingStudent.LastName = updatedStudent.LastName;
            existingStudent.EnrollmentDate = updatedStudent.EnrollmentDate;
            existingStudent.GraduationDate = updatedStudent.GraduationDate;

            await db.SaveChangesAsync();
            return Results.Ok();
        }
    }
}
