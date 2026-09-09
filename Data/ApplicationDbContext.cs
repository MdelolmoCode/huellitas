using Huellitas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Huellitas.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<AdoptionRequest> AdoptionRequests => Set<AdoptionRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var utcConverter = new ValueConverter<DateTime, DateTime>(
            value => value.Kind == DateTimeKind.Utc
            ? value
            : value.ToUniversalTime(),
            value => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        );

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.Property(animal => animal.Name)
            .HasMaxLength(80)
            .IsRequired();

            entity.Property(animal => animal.Breed)
            .HasMaxLength(100);

            entity.Property(animal => animal.Description)
            .HasMaxLength(2000)
            .IsRequired();
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(user => user.DisplayName)
            .HasMaxLength(100)
            .IsRequired();

            entity.Property(user => user.CreatedAt)
            .HasConversion(utcConverter)
            .IsRequired();
        });

        modelBuilder.Entity<AdoptionRequest>(entity =>
        {
            entity.Property(request => request.Reason)
            .HasMaxLength(1000)
            .IsRequired();

            entity.Property(request => request.SubmittedAt)
            .HasConversion(utcConverter)
            .IsRequired();

            entity.HasOne(request => request.Animal)
            .WithMany(animal => animal.AdoptionRequests)
            .HasForeignKey(request => request.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(request => request.User)
            .WithMany(user => user.AdoptionRequests)
            .HasForeignKey(request => request.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
