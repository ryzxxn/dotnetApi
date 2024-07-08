using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using dotnetApi.Models;
namespace dotnetApi.Data
{
    public class ClassDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ClassDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Class> Class { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}
