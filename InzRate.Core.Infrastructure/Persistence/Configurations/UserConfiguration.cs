using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configure the primary key
        builder.HasKey(u => u.Id);
        
        // Configure Id property
        builder.Property(u => u.Id)
            .ValueGeneratedNever(); // Guid will be generated in the application layer
        
        // Configure Username property
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
        
        // Configure unique index on Username
        builder.HasIndex(u => u.Username)
            .IsUnique();
        
        // Table name
        builder.ToTable("Users");
    }
}