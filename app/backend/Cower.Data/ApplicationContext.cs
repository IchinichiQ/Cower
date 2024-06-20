using Cower.Data.Models.Entities;
using Cower.Domain.Models;
using Cower.Domain.Models.Booking;
using Cower.Service.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Cower.Data;

public sealed class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<CoworkingEntity> Coworkings { get; set; }
    public DbSet<CoworkingSeatEntity> CoworkingSeats { get; set; }
    public DbSet<CoworkingWorkingTimeEntity> CoworkingsWorkingTime { get; set; }
    public DbSet<CoworkingFloorEntity> CoworkingFloors { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<ImageEntity> Images { get; set; }
    public DbSet<PasswordResetTokenEntity> PasswordResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<BookingStatus>();
        modelBuilder.HasPostgresEnum<ImageType>();
        
        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { Id = 1, Name = "Admin" },
            new RoleEntity { Id = 2, Name = "User" }
        );

        modelBuilder.Entity<UserEntity>().HasData(
            new UserEntity
            {
                Id = 1,
                Email = "admin@cower.ru",
                PasswordHash =
                    PasswordHashingHelper.HashPassword("lyE2oSz5FhzxCQPsgZqs"),
                RoleId = 1
            });
    }
}