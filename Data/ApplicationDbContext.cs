using Huellitas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Huellitas.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<AdoptionRequest> AdoptionRequests => Set<AdoptionRequest>();
}
