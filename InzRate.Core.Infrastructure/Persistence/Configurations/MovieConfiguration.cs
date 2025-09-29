using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Infrastructure.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        // Configure the primary key
        builder.HasKey(m => m.Id);
        
        // Configure Id property
        builder.Property(m => m.Id)
            .ValueGeneratedNever(); // Guid will be generated in the application layer
        
        // Configure Title property
        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        // Configure ReleaseYear property
        builder.Property(m => m.ReleaseYear)
            .IsRequired();
        
        // Table name
        builder.ToTable("Movies");
    }
}