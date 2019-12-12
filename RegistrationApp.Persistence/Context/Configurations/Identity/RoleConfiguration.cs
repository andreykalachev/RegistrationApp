using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegistrationApp.Domain.Core.Identity;

namespace RegistrationApp.Persistence.Context.Configurations.Identity
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Users).WithOne(x => x.Role);
            builder.HasAlternateKey(x => x.Name);
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
