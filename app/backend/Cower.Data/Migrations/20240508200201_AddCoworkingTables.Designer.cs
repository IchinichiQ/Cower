﻿// <auto-generated />
using System;
using Cower.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cower.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240508200201_AddCoworkingTables")]
    partial class AddCoworkingTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Cower.Data.Entities.BookingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CoworkingAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("coworking_address");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("end_time");

                    b.Property<int>("Floor")
                        .HasColumnType("integer")
                        .HasColumnName("floor");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<long>("SeatId")
                        .HasColumnType("bigint")
                        .HasColumnName("seat_id");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("integer")
                        .HasColumnName("seat_number");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone")
                        .HasColumnName("start_time");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_bookings");

                    b.HasIndex("SeatId")
                        .HasDatabaseName("ix_bookings_seat_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_bookings_user_id");

                    b.ToTable("bookings", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<int>("Floors")
                        .HasColumnType("integer")
                        .HasColumnName("floors");

                    b.HasKey("Id")
                        .HasName("pk_coworkings");

                    b.ToTable("coworkings", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingFloorMediaEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("BackgroundFilename")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("background_filename");

                    b.Property<long>("CoworkingId")
                        .HasColumnType("bigint")
                        .HasColumnName("coworking_id");

                    b.Property<int>("Number")
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    b.HasKey("Id")
                        .HasName("pk_coworking_floors_media");

                    b.HasIndex("CoworkingId")
                        .HasDatabaseName("ix_coworking_floors_media_coworking_id");

                    b.ToTable("coworking_floors_media", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingSeatEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double>("Angle")
                        .HasColumnType("double precision")
                        .HasColumnName("angle");

                    b.Property<long>("CoworkingId")
                        .HasColumnType("bigint")
                        .HasColumnName("coworking_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Floor")
                        .HasColumnType("integer")
                        .HasColumnName("floor");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<string>("ImageFilename")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_filename");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Number"));

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("Width")
                        .HasColumnType("integer")
                        .HasColumnName("width");

                    b.Property<int>("X")
                        .HasColumnType("integer")
                        .HasColumnName("x");

                    b.Property<int>("Y")
                        .HasColumnType("integer")
                        .HasColumnName("y");

                    b.HasKey("Id")
                        .HasName("pk_coworking_seats");

                    b.HasIndex("CoworkingId")
                        .HasDatabaseName("ix_coworking_seats_coworking_id");

                    b.ToTable("coworking_seats", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingWorkingTimeEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<TimeOnly>("Close")
                        .HasColumnType("time without time zone")
                        .HasColumnName("close");

                    b.Property<long>("CoworkingId")
                        .HasColumnType("bigint")
                        .HasColumnName("coworking_id");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer")
                        .HasColumnName("day_of_week");

                    b.Property<TimeOnly>("Open")
                        .HasColumnType("time without time zone")
                        .HasColumnName("open");

                    b.HasKey("Id")
                        .HasName("pk_coworkings_working_time");

                    b.HasIndex("CoworkingId")
                        .HasDatabaseName("ix_coworkings_working_time_coworking_id");

                    b.ToTable("coworkings_working_time", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Entities.RoleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("Cower.Data.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_hash");

                    b.Property<string>("Phone")
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint")
                        .HasColumnName("role_id");

                    b.Property<string>("Surname")
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_users_role_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Entities.BookingEntity", b =>
                {
                    b.HasOne("Cower.Data.Entities.CoworkingSeatEntity", "Seat")
                        .WithMany()
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bookings_coworking_seats_seat_id");

                    b.HasOne("Cower.Data.Entities.UserEntity", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bookings_users_user_id");

                    b.Navigation("Seat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingFloorMediaEntity", b =>
                {
                    b.HasOne("Cower.Data.Entities.CoworkingEntity", "Coworking")
                        .WithMany()
                        .HasForeignKey("CoworkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_coworking_floors_media_coworkings_coworking_id");

                    b.Navigation("Coworking");
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingSeatEntity", b =>
                {
                    b.HasOne("Cower.Data.Entities.CoworkingEntity", "Coworking")
                        .WithMany()
                        .HasForeignKey("CoworkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_coworking_seats_coworkings_coworking_id");

                    b.Navigation("Coworking");
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingWorkingTimeEntity", b =>
                {
                    b.HasOne("Cower.Data.Entities.CoworkingEntity", "Coworking")
                        .WithMany("WorkingTimes")
                        .HasForeignKey("CoworkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_coworkings_working_time_coworkings_coworking_id");

                    b.Navigation("Coworking");
                });

            modelBuilder.Entity("Cower.Data.Entities.UserEntity", b =>
                {
                    b.HasOne("Cower.Data.Entities.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Cower.Data.Entities.CoworkingEntity", b =>
                {
                    b.Navigation("WorkingTimes");
                });

            modelBuilder.Entity("Cower.Data.Entities.UserEntity", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
