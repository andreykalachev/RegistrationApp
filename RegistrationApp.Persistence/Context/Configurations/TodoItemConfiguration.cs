using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegistrationApp.Domain.Core.Entities;

namespace RegistrationApp.Persistence.Context.Configurations
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.AddedBy);
            builder.Property(x => x.DateAdded).IsRequired(false);
            builder.Property(x => x.DateDone).IsRequired(false);
        }
    }
}
