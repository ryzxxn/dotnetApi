using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnetApi.Functions
{
    public static class Query
    {
        public static async Task<IResult> GetStudentDetailsById(int id, MySqlDbContext db)
        {
            try
            {
                Console.WriteLine(id);
                var studentDetails = await db.Students
                    .Where(s => s.StudentID == id)
                    .Select(s => new
                    {
                        s.StudentID,
                        s.FirstName,
                        s.LastName,
                        Courses = s.StudentCourses.Select(sc => new
                        {
                            CourseID = sc.Course.CourseID,
                            CourseName = sc.Course.CourseName,
                            Department = sc.Course.Department != null ? new
                            {
                                sc.Course.Department.DepartmentID,
                                sc.Course.Department.DepartmentName
                            } : null,
                            Instructor = sc.Course.Instructor != null ? new
                            {
                                sc.Course.Instructor.InstructorID,
                                sc.Course.Instructor.FirstName,
                                sc.Course.Instructor.LastName
                            } : null
                        })
                    })
                    .FirstOrDefaultAsync();

                if (studentDetails == null)
                {
                    return Results.NotFound("Student not found");
                }

                return Results.Ok(studentDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching student details: {ex.Message}");
                return Results.BadRequest($"Failed to fetch student details.{ex.Message}");
            }
        }
    }
}
