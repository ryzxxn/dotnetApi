using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace dotnetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly MySqlDbContext _dbContext;

        public AuthenticationController(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email is already registered
            if (await _dbContext.User.AnyAsync(u => u.Email == user.Email))
            {
                return Conflict("Email already exists");
            }

            // Hash the password
            var passwordHash = HashPassword(user.Password);

            // Create new user
            var newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = passwordHash,
                IsActive = user.IsActive,
                Status = user.Status, // Example status
                CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                UserUserRoles = new List<UserUserRole>() // Initialize the navigation property
            };

            _dbContext.User.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok("Added User successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(SignupRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Hash the password from the login request
            var passwordHash = HashPassword(model.Password);

            // Check if the user with the provided email and hashed password exists
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == passwordHash);

            if (user == null)
            {
                return NotFound("Invalid credentials");
            }

            // You can return additional data or tokens here for successful login
            return Ok("Login successful");
        }

        // Helper method to hash the password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
