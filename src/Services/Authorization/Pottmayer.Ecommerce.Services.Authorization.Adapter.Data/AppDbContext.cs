using Microsoft.EntityFrameworkCore;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Users.Entities;

namespace Pottmayer.Ecommerce.Services.Authorization.Adapter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
