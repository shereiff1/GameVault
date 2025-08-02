using GameVault.DAL.Entites;
using GameVault.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GameVault.DAL.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Game> games { get; set; }
        public DbSet<InventoryItem> inventoryItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=SHERIF\\SQLEXPRESS;Database=GameVault;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<InventoryItem>()
       .HasOne(ii => ii.Game)
       .WithMany()
       .HasForeignKey(ii => ii.GameId)
       .OnDelete(DeleteBehavior.Cascade);

            model.Entity<InventoryItem>()
                .HasOne(ii => ii.Inventory)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(model);
        }
    }
}
