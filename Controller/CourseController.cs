using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly MySqlDbContext _db;

        public CourseController(MySqlDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            try
            {
                if (course == null)
                {
                    return BadRequest("Course details cannot be null.");
                }

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(course, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(course, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }

                _db.Courses.Add(course);
                await _db.SaveChangesAsync();
                return Ok(course);
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
                Console.WriteLine($"Error creating course: {ex.Message}");
                return Problem($"Failed to create course: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            try
            {
                var existingCourse = await _db.Courses.FindAsync(id);
                if (existingCourse == null)
                {
                    return NotFound("Course not found");
                }

                existingCourse.CourseName = course.CourseName;
                existingCourse.CourseDescription = course.CourseDescription;
                existingCourse.CourseUnits = course.CourseUnits;
                existingCourse.DepartmentID = course.DepartmentID;
                existingCourse.InstructorID = course.InstructorID;

                await _db.SaveChangesAsync();
                return Ok(existingCourse);
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating course: {ex.Message}");
                return Problem($"Failed to update course: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var existingCourse = await _db.Courses.FindAsync(id);
                if (existingCourse == null)
                {
                    return NotFound("Course not found");
                }

                _db.Courses.Remove(existingCourse);
                await _db.SaveChangesAsync();

                return Ok("Course deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting course: {ex.Message}");
                return Problem($"Failed to delete course: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courseData = _db.Courses.Select(c => new
            {
                c.CourseID,
                c.CourseName,
                c.CourseDescription,
                c.CourseUnits,
                c.DepartmentID,
                c.InstructorID
            }).ToList();

            return Ok(courseData);
        }
    }
}