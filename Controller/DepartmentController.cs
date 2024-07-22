using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly MySqlDbContext _db;

        public DepartmentController(MySqlDbContext db)
        {
            _db = db;
        }

        // GET: api/department
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            try
            {
                var departments = _db.Departments.Select(d => new
                {
                    d.DepartmentID,
                    d.DepartmentName
                }).ToList();

                return Ok(departments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving departments: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve departments");
            }
        }

        // POST: api/department
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Department details cannot be null.");
                }

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(department, serviceProvider: null, items: null);
                bool isValid = Validator.TryValidateObject(department, context, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }

                var existingDepartment = await _db.Departments.FirstOrDefaultAsync(d => d.DepartmentName == department.DepartmentName);
                if (existingDepartment != null)
                {
                    return BadRequest("A department with this name already exists.");
                }

                _db.Departments.Add(department);
                await _db.SaveChangesAsync();

                return Ok(department);
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
                Console.WriteLine($"Error creating department: {ex.Message}");
                return Problem($"Failed to create department: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/department/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Department details cannot be null.");
                }

                var existingDepartment = await _db.Departments.FindAsync(id);
                if (existingDepartment == null)
                {
                    return NotFound("Department not found");
                }

                existingDepartment.DepartmentName = department.DepartmentName;

                await _db.SaveChangesAsync();

                return Ok(existingDepartment);
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating department: {ex.Message}");
                return Problem($"Failed to update department: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/department/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var existingDepartment = await _db.Departments
                    .Include(d => d.Courses)
                    .Include(d => d.Instructors)
                    .FirstOrDefaultAsync(d => d.DepartmentID == id);

                if (existingDepartment == null)
                {
                    return NotFound("Department not found");
                }

                _db.Courses.RemoveRange(existingDepartment.Courses);
                _db.Instructors.RemoveRange(existingDepartment.Instructors);

                _db.Departments.Remove(existingDepartment);
                await _db.SaveChangesAsync();

                return Ok("Department deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return Problem($"Database update error: {dbEx.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting department: {ex.Message}");
                return Problem($"Failed to delete department: {ex.Message}", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
