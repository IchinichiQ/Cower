﻿// <auto-generated />
using System;
using Cower.Data;
using Cower.Domain.Models;
using Cower.Domain.Models.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cower.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240614213038_AddPasswordResetTokenTable")]
    partial class AddPasswordResetTokenTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "booking_status", new[] { "awaiting_payment", "paid", "in_progress", "success", "cancelled", "payment_timeout" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "image_type", new[] { "floor", "seat" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Cower.Data.Models.Entities.BookingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("BookingDate")
                        .HasColumnType("date")
                        .HasColumnName("booking_date");

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

                    b.Property<BookingStatus>("Status")
                        .HasColumnType("booking_status")
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

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingEntity", b =>
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

                    b.HasKey("Id")
                        .HasName("pk_coworkings");

                    b.ToTable("coworkings", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingFloorEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CoworkingId")
                        .HasColumnType("bigint")
                        .HasColumnName("coworking_id");

                    b.Property<long>("ImageId")
                        .HasColumnType("bigint")
                        .HasColumnName("image_id");

                    b.Property<int>("Number")
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    b.HasKey("Id")
                        .HasName("pk_coworking_floors");

                    b.HasIndex("ImageId")
                        .HasDatabaseName("ix_coworking_floors_image_id");

                    b.HasIndex("CoworkingId", "Number")
                        .IsUnique()
                        .HasDatabaseName("ix_coworking_floors_coworking_id_number");

                    b.ToTable("coworking_floors", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingSeatEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double>("Angle")
                        .HasColumnType("double precision")
                        .HasColumnName("angle");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<long>("FloorId")
                        .HasColumnType("bigint")
                        .HasColumnName("floor_id");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<long>("ImageId")
                        .HasColumnType("bigint")
                        .HasColumnName("image_id");

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

                    b.HasIndex("ImageId")
                        .HasDatabaseName("ix_coworking_seats_image_id");

                    b.HasIndex("FloorId", "Number")
                        .IsUnique()
                        .HasDatabaseName("ix_coworking_seats_floor_id_number");

                    b.ToTable("coworking_seats", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingWorkingTimeEntity", b =>
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

            modelBuilder.Entity("Cower.Data.Models.Entities.ImageEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("extension");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<ImageType>("Type")
                        .HasColumnType("image_type")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_images");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.PasswordResetTokenEntity", b =>
                {
                    b.Property<Guid>("Token")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("token");

                    b.Property<DateTimeOffset>("ExpireAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expire_at");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Token")
                        .HasName("pk_password_reset_tokens");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_password_reset_tokens_user_id");

                    b.ToTable("password_reset_tokens", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.PaymentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("BookingId")
                        .HasColumnType("bigint")
                        .HasColumnName("booking_id");

                    b.Property<DateTimeOffset>("ExpireAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expire_at");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_completed");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("label");

                    b.Property<string>("PaymentUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("payment_url");

                    b.HasKey("Id")
                        .HasName("pk_payments");

                    b.HasIndex("BookingId")
                        .IsUnique()
                        .HasDatabaseName("ix_payments_booking_id");

                    b.HasIndex("Label")
                        .IsUnique()
                        .HasDatabaseName("ix_payments_label");

                    b.ToTable("payments", (string)null);
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.RoleEntity", b =>
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

            modelBuilder.Entity("Cower.Data.Models.Entities.UserEntity", b =>
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

            modelBuilder.Entity("Cower.Data.Models.Entities.BookingEntity", b =>
                {
                    b.HasOne("Cower.Data.Models.Entities.CoworkingSeatEntity", "Seat")
                        .WithMany()
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bookings_coworking_seats_seat_id");

                    b.HasOne("Cower.Data.Models.Entities.UserEntity", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bookings_users_user_id");

                    b.Navigation("Seat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingFloorEntity", b =>
                {
                    b.HasOne("Cower.Data.Models.Entities.CoworkingEntity", "Coworking")
                        .WithMany("Floors")
                        .HasForeignKey("CoworkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_coworking_floors_coworkings_coworking_id");

                    b.HasOne("Cower.Data.Models.Entities.ImageEntity", "Image")
                        .WithMany("Floors")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_coworking_floors_images_image_id");

                    b.Navigation("Coworking");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingSeatEntity", b =>
                {
                    b.HasOne("Cower.Data.Models.Entities.CoworkingFloorEntity", "Floor")
                        .WithMany("Seats")
                        .HasForeignKey("FloorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_coworking_seats_coworking_floors_floor_id");

                    b.HasOne("Cower.Data.Models.Entities.ImageEntity", "Image")
                        .WithMany("Seats")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_coworking_seats_images_image_id");

                    b.Navigation("Floor");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingWorkingTimeEntity", b =>
                {
                    b.HasOne("Cower.Data.Models.Entities.CoworkingEntity", "Coworking")
                        .WithMany("WorkingTimes")
                        .HasForeignKey("CoworkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_coworkings_working_time_coworkings_coworking_id");

                    b.Navigation("Coworking");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.PasswordResetTokenEntity", b =>
                {
                    b.HasOne("Cower.Data.Models.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_password_reset_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.PaymentEntity", b =>
                {
                    b.HasOne("Cower.Data.Models.Entities.BookingEntity", "Booking")
                        .WithOne("Payment")
                        .HasForeignKey("Cower.Data.Models.Entities.PaymentEntity", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_payments_bookings_booking_id");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.UserEntity", b =>
                {
                    b.HasOne("Cower.Data.Models.Entities.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.BookingEntity", b =>
                {
                    b.Navigation("Payment");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingEntity", b =>
                {
                    b.Navigation("Floors");

                    b.Navigation("WorkingTimes");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.CoworkingFloorEntity", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.ImageEntity", b =>
                {
                    b.Navigation("Floors");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("Cower.Data.Models.Entities.UserEntity", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
