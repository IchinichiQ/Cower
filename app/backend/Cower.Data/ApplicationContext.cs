using Cower.Data.Models.Entities;
using Cower.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data;

public sealed class ApplicationContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<CoworkingEntity> Coworkings { get; set; }
    public DbSet<CoworkingSeatEntity> CoworkingSeats { get; set; }
    public DbSet<CoworkingWorkingTimeEntity> CoworkingsWorkingTime { get; set; }
    public DbSet<CoworkingFloorMediaEntity> CoworkingFloorsMedia { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { Id = 1, Name = "Admin" },
            new RoleEntity { Id = 2, Name = "User" }
        );
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
            .UseSnakeCaseNamingConvention();
    }

}