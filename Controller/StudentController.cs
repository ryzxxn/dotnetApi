using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetApi;
using Microsoft.AspNetCore.Http;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly MySqlDbContext _db;

        public StudentController(MySqlDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                List<Student> studentdata = await _db.Students.ToListAsync();
                return Ok(studentdata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching students: {ex.Message}");
                return StatusCode(500, "Failed to fetch students.");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            try
            {
                _db.Students.Add(student);
                await _db.SaveChangesAsync();
                return Ok(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating student: {ex.Message}");
                return BadRequest("Failed to create student.");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteStudent([FromBody] Student student)
        {
            try
            {
                var existingStudent = await _db.Students.FindAsync(student.StudentID);
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                _db.Students.Remove(existingStudent);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return StatusCode(500, "Failed to delete student.");
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStudent([FromBody] Student updatedStudent, MySqlDbContext db)
        {
            try
            {
                var existingStudent = await _db.Students.FindAsync(updatedStudent.StudentID);
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                 if (updatedStudent.FirstName == null || updatedStudent.LastName == null || updatedStudent.GraduationDate == null || updatedStudent.EnrollmentDate == null) 
                {
                    return StatusCode(500, "missing values");
                }
                else{
                    // Update the existing student row with the new values
                    existingStudent.FirstName = updatedStudent.FirstName;
                    existingStudent.LastName = updatedStudent.LastName;
                    existingStudent.LastName = updatedStudent.EnrollmentDate;
                    existingStudent.LastName = updatedStudent.GraduationDate;

                    // Save changes to the database
                    await db.SaveChangesAsync();
                    return StatusCode(200, "Sucessful");;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student: {ex.Message}");
                return BadRequest("Failed to update student.");
            }
        }
    }
}
