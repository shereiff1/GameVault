
using Microsoft.EntityFrameworkCore;

namespace GameVault.DAL.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        // Bassam Dina Ibrahem => change only the server name to your server name

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=SHERIF\\SQLEXPRESS;Database=GameVault;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        }

        // fluent API
        protected override void OnModelCreating(ModelBuilder model)
        {

        }
    }
}
