using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using dotnetApi.Models;
namespace dotnetApi.Data
{
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}
