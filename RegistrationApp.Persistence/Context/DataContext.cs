using Microsoft.EntityFrameworkCore;
using RegistrationApp.Domain.Core.Entities;
using RegistrationApp.Domain.Core.Identity;
using RegistrationApp.Persistence.Context.Configurations;
using RegistrationApp.Persistence.Context.Configurations.Identity;

namespace RegistrationApp.Persistence.Context
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
        }
    }
}
