using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly MySqlDbContext _db;

        public StudentController(MySqlDbContext db)
        {
            _db = db;
        }

        // GET: api/student
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            try
            {
                var students = _db.Students.Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    s.EnrollmentDate,
                    s.GraduationDate
                }).ToList();

                return Ok(students);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving students: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve students");
            }
        }

        // POST: api/student
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Student details cannot be null.");
                }

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(student, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(student, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }

                _db.Students.Add(student);
                await _db.SaveChangesAsync();

                return Ok(student);
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (ValidationException valEx)
            {
                Console.WriteLine($"Validation error: {valEx.Message}");
                return BadRequest($"Validation error: {valEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating student: {ex.Message}");
                return Problem($"Failed to create student: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/student/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var existingStudent = await _db.Students.FindAsync(id);
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                _db.Students.Remove(existingStudent);
                await _db.SaveChangesAsync();

                return Ok("Student deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return Problem($"Failed to delete student: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/student/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Student details cannot be null.");
                }

                var existingStudent = await _db.Students.FirstOrDefaultAsync(s => s.StudentID == id);
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.EnrollmentDate = student.EnrollmentDate;
                existingStudent.GraduationDate = student.GraduationDate;

                await _db.SaveChangesAsync();

                return Ok("Student updated successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student: {ex.Message}");
                return Problem($"Failed to update student: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
