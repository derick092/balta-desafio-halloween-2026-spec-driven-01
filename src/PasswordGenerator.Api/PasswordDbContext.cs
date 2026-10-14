using Microsoft.EntityFrameworkCore;

namespace PasswordGenerator.Api;

public sealed class PasswordDbContext(DbContextOptions<PasswordDbContext> options)
    : DbContext(options)
{
    public DbSet<PasswordEntry> Passwords => Set<PasswordEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PasswordEntry>(entity =>
        {
            entity.HasKey(password => password.Id);
            entity.Property(password => password.Value).IsRequired();
        });
    }
}