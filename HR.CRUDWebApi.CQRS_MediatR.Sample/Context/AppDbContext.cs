using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=host.docker.internal;Database=DemoApp;User Id=sa;Password=Admin1234!;TrustServerCertificate=True;");
        }
    }
}
