using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
