using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace dotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly MySqlDbContext _db;

        public InstructorController(MySqlDbContext db)
        {
            _db = db;
        }

        // GET: api/instructor
        [HttpGet]
        public IActionResult GetAllInstructors()
        {
            try
            {
                var instructors = _db.Instructors.Select(i => new
                {
                    i.InstructorID,
                    i.FirstName,
                    i.LastName,
                    i.Status,
                    i.HireDate,
                    i.AnnualSalary,
                    i.DepartmentID
                }).ToList();

                Console.WriteLine($"Retrieved Instructors: {JsonSerializer.Serialize(instructors)}");

                return Ok(instructors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving instructors: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve instructors");
            }
        }

        // GET: api/instructor/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorById(int id)
        {
            try
            {
                var instructor = await _db.Instructors
                    .Include(i => i.Department)
                    .FirstOrDefaultAsync(i => i.InstructorID == id);

                if (instructor == null)
                {
                    return NotFound("Instructor not found");
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

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving instructor: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve instructor");
            }
        }

        // POST: api/instructor
        [HttpPost]
        public async Task<IActionResult> CreateInstructor([FromBody] Instructor instructor)
        {
            try
            {
                Console.WriteLine($"Received Instructor: {JsonSerializer.Serialize(instructor)}");

                if (instructor == null)
                {
                    return BadRequest("Instructor details cannot be null.");
                }

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(instructor, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(instructor, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    Console.WriteLine($"Validation errors: {JsonSerializer.Serialize(errors)}");
                    return BadRequest(new { Errors = errors });
                }

                _db.Instructors.Add(instructor);
                await _db.SaveChangesAsync();

                return Ok(instructor);
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
                Console.WriteLine($"Error creating instructor: {ex.Message}");
                return Problem($"Failed to create instructor: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/instructor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructor(int id, [FromBody] Instructor instructor)
        {
            try
            {
                Console.WriteLine($"Received Instructor: {JsonSerializer.Serialize(instructor)}");

                var existingInstructor = await _db.Instructors.FindAsync(id);
                if (existingInstructor == null)
                {
                    return NotFound("Instructor not found");
                }

                existingInstructor.FirstName = instructor.FirstName;
                existingInstructor.LastName = instructor.LastName;
                existingInstructor.Status = instructor.Status;
                existingInstructor.HireDate = instructor.HireDate;
                existingInstructor.AnnualSalary = instructor.AnnualSalary;
                existingInstructor.DepartmentID = instructor.DepartmentID;

                await _db.SaveChangesAsync();

                return Ok(existingInstructor);
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating instructor: {ex.Message}");
                return Problem($"Failed to update instructor: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/instructor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            try
            {
                Console.WriteLine($"Deleting Instructor ID: {id}");

                var existingInstructor = await _db.Instructors
                    .Include(i => i.Department)
                    .FirstOrDefaultAsync(i => i.InstructorID == id);

                if (existingInstructor == null)
                {
                    return NotFound("Instructor not found");
                }

                _db.Instructors.Remove(existingInstructor);
                await _db.SaveChangesAsync();

                return Ok("Instructor deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting instructor: {ex.Message}");
                return Problem($"Failed to delete instructor: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
