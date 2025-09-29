using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InzRate.Core.Domain.Aggregates;
using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        // Configure the primary key
        builder.HasKey(r => r.Id);
        
        // Configure Id property
        builder.Property(r => r.Id)
            .ValueGeneratedNever(); // Guid will be generated in the application layer
        
        // Configure MovieId property
        builder.Property(r => r.MovieId)
            .IsRequired();
        
        // Configure UserId property
        builder.Property(r => r.UserId)
            .IsRequired();
        
        // Configure ReviewText property
        builder.Property(r => r.ReviewText)
            .IsRequired()
            .HasMaxLength(1000);
        
        // Configure CreatedAt property
        builder.Property(r => r.CreatedAt)
            .IsRequired();
        
        // Configure foreign key constraints at the database level
        // This enforces referential integrity without creating navigation properties in the domain
        builder.HasOne<User>()  // This creates a reference to User without exposing it in the domain
            .WithMany()         // No navigation property in the domain entity
            .HasForeignKey(r => r.UserId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.NoAction); // Prevents cascade delete for users

        builder.HasOne<Movie>() // This creates a reference to Movie without exposing it in the domain
            .WithMany()         // No navigation property in the domain entity
            .HasForeignKey(r => r.MovieId)
            .HasPrincipalKey(m => m.Id)
            .OnDelete(DeleteBehavior.Cascade); // Allows cascade delete for movies
        
        // Table name
        builder.ToTable("Reviews");
    }
}