using Microsoft.AspNetCore.Mvc;
using dotnetApi.Models;
using dotnetApi.Data;

namespace dotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ClassDbContext _context;

        public ClassController(ClassDbContext context)
        {
            _context = context;
        }

        // GET: api/Class
        [HttpGet("GetClasses")]
        public ActionResult<IEnumerable<Class>> GetClasses()
        {
            return Ok(_context.Classes.ToList());
        }
    }
}