using LAP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LAP_API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<VehicleGroup> VehicleGroups => Set<VehicleGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VehicleGroup>(e =>
        {
            e.HasKey(x => new { x.GroupId, x.VehicleId });
        });
    }
}
