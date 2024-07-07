using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetApi.Models;
using dotnetApi.Data;

namespace dotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly MyDbContext _context;

        public StudentController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            return Ok(_context.Students.ToList());
        }

        // PUT api/Student/{rollNumber}
        [HttpPut("{rollNumber}")]
        public IActionResult Put(int rollNumber, Student student)
        {
            if (rollNumber != student.RollNumber)
            {
                return BadRequest();
            }

            _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok("Updated Student Details");
        }

        // DELETE api/Student/{id}
        [HttpDelete("{StudentId}")]
        public IActionResult Delete(Guid StudentId)
        {
            var student = _context.Students.Find(StudentId);
            if (student == null)
            {
                return Ok($"Student {StudentId} does not exist.");
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return Ok($"Deleted student {StudentId}.");
        }

        // CREATE api/Student
        [HttpPost]
        public IActionResult Create(Student student)
        {
            student.StudentId = Guid.NewGuid();
            _context.Students.Add(student);
            _context.SaveChanges();

            return CreatedAtAction("Get", new { id = student.StudentId }, student);
        }
    }
}
