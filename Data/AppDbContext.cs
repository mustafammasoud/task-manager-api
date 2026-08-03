using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models.Entities;

namespace TaskManagerApi.Data;

/// <summary>
/// EF Core context for the PostgreSQL-backed task store. Kept minimal:
/// just the DbSet and the model configuration needed to map TaskItem
/// onto a sensible table shape.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("tasks");
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .HasMaxLength(2000);

            // Stored as the enum's string name ("Pending" / "Completed")
            // rather than an int, so the raw table data is self-explanatory.
            entity.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired();
        });
    }
}
